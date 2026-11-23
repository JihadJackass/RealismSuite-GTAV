using GTA;
using SPAII.INM;

namespace SPAII
{
	public class WardrobeMemory
	{
		private ClothingSet.CS _face;

		private ClothingSet.CS _beard;

		private ClothingSet.CS _hair;

		private ClothingSet.CS _torso;

		private ClothingSet.CS _legs;

		private ClothingSet.CS _hands;

		private ClothingSet.CS _feet;

		private ClothingSet.CS _eyes;

		private ClothingSet.CS _accessories;

		private ClothingSet.CS _props;

		private ClothingSet.CS _textures;

		private ClothingSet.CS _auxTorso;

		private ClothingSet.CS _helmet;

		private ClothingSet.CS _glasses;

		private ClothingSet.CS _earrings;

		public ClothingSet.CS Face
		{
			get
			{
				return _face;
			}
			set
			{
				_face = value;
			}
		}

		public ClothingSet.CS Beard
		{
			get
			{
				return _beard;
			}
			set
			{
				_beard = value;
			}
		}

		public ClothingSet.CS Hair
		{
			get
			{
				return _hair;
			}
			set
			{
				_hair = value;
			}
		}

		public ClothingSet.CS Torso
		{
			get
			{
				return _torso;
			}
			set
			{
				_torso = value;
			}
		}

		public ClothingSet.CS Legs
		{
			get
			{
				return _legs;
			}
			set
			{
				_legs = value;
			}
		}

		public ClothingSet.CS Hands
		{
			get
			{
				return _hands;
			}
			set
			{
				_hands = value;
			}
		}

		public ClothingSet.CS Feet
		{
			get
			{
				return _feet;
			}
			set
			{
				_feet = value;
			}
		}

		public ClothingSet.CS Eyes
		{
			get
			{
				return _eyes;
			}
			set
			{
				_eyes = value;
			}
		}

		public ClothingSet.CS Accessories
		{
			get
			{
				return _accessories;
			}
			set
			{
				_accessories = value;
			}
		}

		public ClothingSet.CS Props
		{
			get
			{
				return _props;
			}
			set
			{
				_props = value;
			}
		}

		public ClothingSet.CS Textures
		{
			get
			{
				return _textures;
			}
			set
			{
				_textures = value;
			}
		}

		public ClothingSet.CS AuxiliaryTorso
		{
			get
			{
				return _auxTorso;
			}
			set
			{
				_auxTorso = value;
			}
		}

		public ClothingSet.CS Helmet
		{
			get
			{
				return _helmet;
			}
			set
			{
				_helmet = value;
			}
		}

		public ClothingSet.CS Glasses
		{
			get
			{
				return _glasses;
			}
			set
			{
				_glasses = value;
			}
		}

		public ClothingSet.CS Earrings
		{
			get
			{
				return _earrings;
			}
			set
			{
				_earrings = value;
			}
		}

		public WardrobeMemory()
		{
		}

		public WardrobeMemory(Ped ped)
		{
			Face = Helper.GetClothes(ped, ePedVariation.Face);
			Beard = Helper.GetClothes(ped, ePedVariation.Beard);
			Hair = Helper.GetClothes(ped, ePedVariation.Hair);
			Torso = Helper.GetClothes(ped, ePedVariation.Torso);
			Legs = Helper.GetClothes(ped, ePedVariation.Legs);
			Hands = Helper.GetClothes(ped, ePedVariation.Hands);
			Feet = Helper.GetClothes(ped, ePedVariation.Feet);
			Eyes = Helper.GetClothes(ped, ePedVariation.Eyes);
			Accessories = Helper.GetClothes(ped, ePedVariation.Accessories);
			Props = Helper.GetClothes(ped, ePedVariation.Props);
			Textures = Helper.GetClothes(ped, ePedVariation.Textures);
			AuxiliaryTorso = Helper.GetClothes(ped, ePedVariation.AuxiliaryTorso);
			Helmet = Helper.GetProps(ped, ePropVariation.Helmet);
			Glasses = Helper.GetProps(ped, ePropVariation.Glasses);
			Earrings = Helper.GetProps(ped, ePropVariation.Earrings);
		}

		public void SetClothes(ClothingSet.CS cs)
		{
			switch (cs.ComponentID)
			{
			case 0:
				Face = cs;
				break;
			case 1:
				Beard = cs;
				break;
			case 2:
				Hair = cs;
				break;
			case 3:
				Torso = cs;
				break;
			case 4:
				Legs = cs;
				break;
			case 5:
				Hands = cs;
				break;
			case 6:
				Feet = cs;
				break;
			case 7:
				Eyes = cs;
				break;
			case 8:
				Accessories = cs;
				break;
			case 9:
				Props = cs;
				break;
			case 10:
				Textures = cs;
				break;
			case 11:
				AuxiliaryTorso = cs;
				break;
			}
		}

		public void SetProps(ClothingSet.CS cs)
		{
			switch (cs.ComponentID)
			{
			case 0:
				Helmet = cs;
				break;
			case 1:
				Glasses = cs;
				break;
			case 2:
				Earrings = cs;
				break;
			}
		}
	}
}
