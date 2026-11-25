// Added for RealismSuite: lets the online vehicle shops deliver a bought car straight into one of your SPA II garages.
// The record is written the same way SPA II writes it when you drive a car into the garage yourself: a VehicleClass made
// from a real vehicle (so colours and mods are read by SPA II's own code), saved as <uid>.xml in the apartment's
// storage folder, in the first free slot. Executive (60-car) and 20-car garages and mansions are left out: they keep
// their own inventories.
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GTA;

namespace SPAII
{
    internal sealed class GarageDestination
    {
        public BuildingClass Building;
        public ApartmentClass Apartment;
        public string Label;
        public int Free, Capacity;
    }

    internal static class ShopDelivery
    {
        // SPA II garages of the character you're playing, with at least one free space
        public static List<GarageDestination> Destinations()
        {
            var list = new List<GarageDestination>();
            try
            {
                if (!Helper.buildingsLoaded) return list;
                eOwner me = Helper.GetPlayer();
                if (!PropertyProfiles.IsCharacter(me)) return list;
                foreach (BuildingClass b in Helper.buildings.ToList())
                {
                    if (b == null || b.Apartments == null) continue;
                    if (b.GarageType != eGarageType.TwoCarGarage && b.GarageType != eGarageType.SixCarGarage && b.GarageType != eGarageType.TenCarGarage) continue;
                    foreach (ApartmentClass a in b.Apartments)
                    {
                        if (a == null || a.ID == 9310 || a.Owner() != me || string.IsNullOrWhiteSpace(a.GarageFilePath)) continue;
                        int cap = b.GetGarageVehicleCount();
                        HashSet<int> used = Used(b, a, me, cap);
                        if (used == null) continue;
                        int free = Enumerable.Range(0, cap).Count(i => !used.Contains(i));
                        if (free <= 0) continue;
                        string label = b.Name;
                        if (b.Apartments.Count > 1)
                        {
                            string apt = Game.GetLocalizedString(a.Name);
                            if (!string.IsNullOrEmpty(apt) && apt != a.Name && apt != b.Name) label = apt;
                        }
                        list.Add(new GarageDestination { Building = b, Apartment = a, Label = label, Free = free, Capacity = cap });
                    }
                }
            }
            catch (Exception ex) { Logger.Log("Shop delivery: listing garages failed: " + ex.Message); }
            return list.OrderBy(d => d.Label).ToList();
        }

        // which slots are taken (null = the garage records can't be read safely)
        static HashSet<int> Used(BuildingClass b, ApartmentClass a, eOwner me, int cap)
        {
            if (a.ApartmentType == eApartmentType.Other && a.HasCharacterProfile())
            {
                HashSet<int> reserved = null;
                if (!a.TryReadStandaloneGarageSlots(me, cap, ref reserved)) return null;
                return reserved ?? new HashSet<int>();
            }
            return new HashSet<int>(a.Vehicles().Select(v => v.Index));
        }

        // write the vehicle into the garage; the caller deletes the vehicle afterwards
        public static bool Deliver(GarageDestination d, Vehicle vehicle, out string error)
        {
            error = null;
            try
            {
                eOwner me = Helper.GetPlayer();
                if (d == null || vehicle == null || !vehicle.Exists()) { error = "the vehicle wasn't ready"; return false; }
                if (d.Apartment.Owner() != me) { error = "that garage isn't yours"; return false; }
                int cap = d.Building.GetGarageVehicleCount();
                HashSet<int> used = Used(d.Building, d.Apartment, me, cap);
                if (used == null) { error = "the garage records need checking"; return false; }
                int slot = Enumerable.Range(0, cap).Where(i => !used.Contains(i)).DefaultIfEmpty(-1).First();
                if (slot < 0) { error = "the garage is full"; return false; }
                string folder = d.Apartment.StorageFolder(PropertyProfiles.ActiveCharacter);
                if (folder == null) { error = "the garage has no storage folder"; return false; }
                string dir = Helper.grgXmlPath + folder;
                Directory.CreateDirectory(dir);
                int uid;
                do uid = Guid.NewGuid().GetHashCode();
                while (uid == 0 || File.Exists(Path.Combine(dir, uid + ".xml")) || File.Exists(Path.Combine(dir, uid + ".xml.bak")));
                var data = new VehicleData(Path.Combine(dir, uid + ".xml"), new VehicleClass(vehicle, me, d.Apartment.ID, uid, slot));
                data.Save();
                Logger.Log("Shop delivery: " + vehicle.Model.Hash + " to " + d.Label + " (apt " + d.Apartment.ID + "), slot " + slot + ", uid " + uid);
                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                Logger.Log("Shop delivery failed: " + ex.Message);
                return false;
            }
        }
    }
}
