using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;

namespace CayoPericoHeistInSP
{
	public class CayoPericoSubTable : Script
	{
		public class ButtonData
		{
			public int ButtonID { get; set; }

			public string DisplayName { get; set; }

			public string KeyboardKey { get; set; }

			public string ControllerKey { get; set; }

			public ButtonData()
			{
			}

			public ButtonData(int bid, string dn, string kk, string ck)
			{
				ButtonID = bid;
				DisplayName = dn;
				KeyboardKey = kk;
				ControllerKey = ck;
			}
		}

		public static bool UseScaleformVarInsteadOfInt = true;

		public static Scaleform SubTableScale;

		private int iLocal_1424;

		private int iLocal_1426;

		public int iLocal_1425;

		public static List<List<dynamic>> SetupItems = new List<List<object>>();

		public static List<List<dynamic>> IntelItems = new List<List<object>>();

		public static List<List<dynamic>> FinaleItems = new List<List<object>>();

		public static Scaleform GUIscaleform;

		public bool ShowingFullMap = true;

		public bool ShowingOverlay = true;

		public int CayoTableSelection_Tab;

		public bool SelectedLaunchButton;

		public int CayoTableSelection_MenuID;

		public int CayoTableSelection_SelectionID;

		public int CayoTableSelection_FinaleSelectionID;

		public int CayoTableSelection_FinaleItemSelection = 64;

		public int CayoTableSelection_FinaleMenuOpenned;

		public int CayoTableSelection_FinaleMenuID;

		public int CayoTableSelection_MenuTabOpen;

		public int CayoTableSelection_SubMenuTabOpen;

		public int CayoTableSelection_SubSubMenuTabOpen;

		public string CurrentCayoSelection = "GATHER INTEL";

		public string SelectedButtonIndex;

		public List<ButtonData> ButtonIndex = new List<ButtonData>
		{
			new ButtonData(0, "INPUT_NEXT_CAMERA", "V", "BACK"),
			new ButtonData(1, "INPUT_LOOK_LR", "MOUSE RIGHT", "RIGHT STICK"),
			new ButtonData(2, "INPUT_LOOK_UD", "MOUSE DOWN", "RIGHT STICK"),
			new ButtonData(3, "INPUT_LOOK_UP_ONLY", "(NONE)", "RIGHT STICK"),
			new ButtonData(4, "INPUT_LOOK_DOWN_ONLY", "MOUSE DOWN", "RIGHT STICK"),
			new ButtonData(5, "INPUT_LOOK_LEFT_ONLY", "(NONE)", "RIGHT STICK"),
			new ButtonData(6, "INPUT_LOOK_RIGHT_ONLY", "MOUSE RIGHT", "RIGHT STICK"),
			new ButtonData(7, "INPUT_CINEMATIC_SLOWMO", "(NONE)", "R3"),
			new ButtonData(8, "INPUT_SCRIPTED_FLY_UD", "S", "LEFT STICK"),
			new ButtonData(9, "INPUT_SCRIPTED_FLY_LR", "D", "LEFT STICK"),
			new ButtonData(10, "INPUT_SCRIPTED_FLY_ZUP", "PAGEUP", "LT"),
			new ButtonData(11, "INPUT_SCRIPTED_FLY_ZDOWN", "PAGEDOWN", "RT"),
			new ButtonData(12, "INPUT_WEAPON_WHEEL_UD", "MOUSE DOWN", "RIGHT STICK"),
			new ButtonData(13, "INPUT_WEAPON_WHEEL_LR", "MOUSE RIGHT", "RIGHT STICK"),
			new ButtonData(14, "INPUT_WEAPON_WHEEL_NEXT", "SCROLLWHEEL DOWN", "DPAD RIGHT"),
			new ButtonData(15, "INPUT_WEAPON_WHEEL_PREV", "SCROLLWHEEL UP", "DPAD LEFT"),
			new ButtonData(16, "INPUT_SELECT_NEXT_WEAPON", "SCROLLWHEEL", "DOWN (NONE)"),
			new ButtonData(17, "INPUT_SELECT_PREV_WEAPON", "SCROLLWHEEL", "UP (NONE)"),
			new ButtonData(18, "INPUT_SKIP_CUTSCENE", "ENTER / LEFT MOUSE BUTTON/SPACEBAR", "A"),
			new ButtonData(19, "INPUT_CHARACTER_WHEEL", "LEFT ALT", "DPAD DOWN"),
			new ButtonData(20, "INPUT_MULTIPLAYER_INFO", "Z", "DPAD DOWN"),
			new ButtonData(21, "INPUT_SPRINT", "LEFT", "SHIFT A"),
			new ButtonData(22, "INPUT_JUMP", "SPACEBAR", "X"),
			new ButtonData(23, "INPUT_ENTER", "F", "Y"),
			new ButtonData(24, "INPUT_ATTACK", "LEFT MOUSE", "BUTTON RT"),
			new ButtonData(25, "INPUT_AIM", "RIGHT MOUSE", "BUTTON LT"),
			new ButtonData(26, "INPUT_LOOK_BEHIND", "C", "R3"),
			new ButtonData(27, "INPUT_PHONE", "ARROW UP / SCROLLWHEEL BUTTON(PRESS)DPAD", "UP"),
			new ButtonData(28, "INPUT_SPECIAL_ABILITY", "(NONE)", "L3"),
			new ButtonData(29, "INPUT_SPECIAL_ABILITY_SECONDARY", "B", "R3"),
			new ButtonData(30, "INPUT_MOVE_LR", "D", "LEFT STICK"),
			new ButtonData(31, "INPUT_MOVE_UD", "S", "LEFT STICK"),
			new ButtonData(32, "INPUT_MOVE_UP_ONLY", "W", "LEFT STICK"),
			new ButtonData(33, "INPUT_MOVE_DOWN_ONLY", "S", "LEFT STICK"),
			new ButtonData(34, "INPUT_MOVE_LEFT_ONLY", "A", "LEFT STICK"),
			new ButtonData(35, "INPUT_MOVE_RIGHT_ONLY", "D", "LEFT STICK"),
			new ButtonData(36, "INPUT_DUCK", "LEFT", "CTRL L3"),
			new ButtonData(37, "INPUT_SELECT_WEAPON", "TAB", "LB"),
			new ButtonData(38, "INPUT_PICKUP", "E", "LB"),
			new ButtonData(39, "INPUT_SNIPER_ZOOM", "[LEFT", "STICK"),
			new ButtonData(40, "INPUT_SNIPER_ZOOM_IN_ONLY]", "LEFT", "STICK"),
			new ButtonData(41, "INPUT_SNIPER_ZOOM_OUT_ONLY", "[LEFT", "STICK"),
			new ButtonData(42, "INPUT_SNIPER_ZOOM_IN_SECONDARY]", "DPAD", "UP"),
			new ButtonData(43, "INPUT_SNIPER_ZOOM_OUT_SECONDARY", "[DPAD", "DOWN"),
			new ButtonData(44, "INPUT_COVER", "Q", "RB"),
			new ButtonData(45, "INPUT_RELOAD", "R", "B"),
			new ButtonData(46, "INPUT_TALK", "E", "DPAD RIGHT"),
			new ButtonData(47, "INPUT_DETONATE", "G", "DPAD LEFT"),
			new ButtonData(48, "INPUT_HUD_SPECIAL", "Z", "DPAD DOWN"),
			new ButtonData(49, "INPUT_ARREST", "F", "Y"),
			new ButtonData(50, "INPUT_ACCURATE_AIM", "SCROLLWHEEL", "DOWN R3"),
			new ButtonData(51, "INPUT_CONTEXT", "E", "DPAD RIGHT"),
			new ButtonData(52, "INPUT_CONTEXT_SECONDARY", "Q", "DPAD LEFT"),
			new ButtonData(53, "INPUT_WEAPON_SPECIAL", "(NONE)", "Y"),
			new ButtonData(54, "INPUT_WEAPON_SPECIAL_TWO", "E", "DPAD RIGHT"),
			new ButtonData(55, "INPUT_DIVE", "SPACEBAR", "RB"),
			new ButtonData(56, "INPUT_DROP_WEAPON", "F9", "Y"),
			new ButtonData(57, "INPUT_DROP_AMMO", "F10", "B"),
			new ButtonData(58, "INPUT_THROW_GRENADE", "G", "DPAD LEFT"),
			new ButtonData(59, "INPUT_VEH_MOVE_LR", "D", "LEFT STICK"),
			new ButtonData(60, "INPUT_VEH_MOVE_UD", "LEFT CTRL", "LEFT STICK"),
			new ButtonData(61, "INPUT_VEH_MOVE_UP_ONLY", "LEFT SHIFT", "LEFT STICK"),
			new ButtonData(62, "INPUT_VEH_MOVE_DOWN_ONLY", "LEFT CTRL", "LEFT STICK"),
			new ButtonData(63, "INPUT_VEH_MOVE_LEFT_ONLY", "A", "LEFT STICK"),
			new ButtonData(64, "INPUT_VEH_MOVE_RIGHT_ONLY", "D", "LEFT STICK"),
			new ButtonData(65, "INPUT_VEH_SPECIAL", "(NONE)", "(NONE)"),
			new ButtonData(66, "INPUT_VEH_GUN_LR", "MOUSE RIGHT", "RIGHT STICK"),
			new ButtonData(67, "INPUT_VEH_GUN_UD", "MOUSE DOWN", "RIGHT STICK"),
			new ButtonData(68, "INPUT_VEH_AIM", "RIGHT MOUSE", "BUTTON LB"),
			new ButtonData(69, "INPUT_VEH_ATTACK", "LEFT MOUSE", "BUTTON RB"),
			new ButtonData(70, "INPUT_VEH_ATTACK2", "RIGHT MOUSE", "BUTTON A"),
			new ButtonData(71, "INPUT_VEH_ACCELERATE", "W", "RT"),
			new ButtonData(72, "INPUT_VEH_BRAKE", "S", "LT"),
			new ButtonData(73, "INPUT_VEH_DUCK", "X", "A"),
			new ButtonData(74, "INPUT_VEH_HEADLIGHT", "H", "DPAD RIGHT"),
			new ButtonData(75, "INPUT_VEH_EXIT", "F", "Y"),
			new ButtonData(76, "INPUT_VEH_HANDBRAKE", "SPACEBAR", "RB"),
			new ButtonData(77, "INPUT_VEH_HOTWIRE_LEFT", "W", "LT"),
			new ButtonData(78, "INPUT_VEH_HOTWIRE_RIGHT", "S", "RT"),
			new ButtonData(79, "INPUT_VEH_LOOK_BEHIND", "C", "R3"),
			new ButtonData(80, "INPUT_VEH_CIN_CAM", "R", "B"),
			new ButtonData(81, "INPUT_VEH_NEXT_RADIO", ".", "(NONE)"),
			new ButtonData(82, "INPUT_VEH_PREV_RADIO", ",", "(NONE)"),
			new ButtonData(83, "INPUT_VEH_NEXT_RADIO_TRACK", "=", "(NONE)"),
			new ButtonData(84, "INPUT_VEH_PREV_RADIO_TRACK", "-", "(NONE)"),
			new ButtonData(85, "INPUT_VEH_RADIO_WHEEL", "Q", "DPAD LEFT"),
			new ButtonData(86, "INPUT_VEH_HORN", "E", "L3"),
			new ButtonData(87, "INPUT_VEH_FLY_THROTTLE_UP", "W", "RT"),
			new ButtonData(88, "INPUT_VEH_FLY_THROTTLE_DOWN", "S", "LT"),
			new ButtonData(89, "INPUT_VEH_FLY_YAW_LEFT", "A", "LB"),
			new ButtonData(90, "INPUT_VEH_FLY_YAW_RIGHT", "D", "RB"),
			new ButtonData(91, "INPUT_VEH_PASSENGER_AIM", "RIGHT MOUSE", "BUTTON LT"),
			new ButtonData(92, "INPUT_VEH_PASSENGER_ATTACK", "LEFT MOUSE", "BUTTON RT"),
			new ButtonData(93, "INPUT_VEH_SPECIAL_ABILITY_FRANKLIN", "(NONE)", "R3"),
			new ButtonData(94, "INPUT_VEH_STUNT_UD", "(NONE)", "(NONE)"),
			new ButtonData(95, "INPUT_VEH_CINEMATIC_UD", "MOUSE DOWN", "RIGHT STICK"),
			new ButtonData(96, "INPUT_VEH_CINEMATIC_UP_ONLY", "NUMPAD- /", "SCROLLWHEEL UP(NONE)"),
			new ButtonData(97, "INPUT_VEH_CINEMATIC_DOWN_ONLY", "NUMPAD+ /", "SCROLLWHEEL DOWN(NONE)"),
			new ButtonData(98, "INPUT_VEH_CINEMATIC_LR", "MOUSE RIGHT", "RIGHT STICK"),
			new ButtonData(99, "INPUT_VEH_SELECT_NEXT_WEAPON", "SCROLLWHEEL", "UP X"),
			new ButtonData(100, "INPUT_VEH_SELECT_PREV_WEAPON", "[", "(NONE)"),
			new ButtonData(101, "INPUT_VEH_ROOF", "H", "DPAD RIGHT"),
			new ButtonData(102, "INPUT_VEH_JUMP", "SPACEBAR", "RB"),
			new ButtonData(103, "INPUT_VEH_GRAPPLING_HOOK", "E", "DPAD RIGHT"),
			new ButtonData(104, "INPUT_VEH_SHUFFLE", "H", "DPAD RIGHT"),
			new ButtonData(105, "INPUT_VEH_DROP_PROJECTILE", "X", "A"),
			new ButtonData(106, "INPUT_VEH_MOUSE_CONTROL_OVERRIDE", "LEFT", "MOUSE BUTTON(NONE)"),
			new ButtonData(107, "INPUT_VEH_FLY_ROLL_LR", "NUMPAD 6", "LEFT STICK"),
			new ButtonData(108, "INPUT_VEH_FLY_ROLL_LEFT_ONLY", "NUMPAD 4", "LEFT STICK"),
			new ButtonData(109, "INPUT_VEH_FLY_ROLL_RIGHT_ONLY", "NUMPAD 6", "LEFT STICK"),
			new ButtonData(110, "INPUT_VEH_FLY_PITCH_UD", "NUMPAD 5", "LEFT STICK"),
			new ButtonData(111, "INPUT_VEH_FLY_PITCH_UP_ONLY", "NUMPAD 8", "LEFT STICK"),
			new ButtonData(112, "INPUT_VEH_FLY_PITCH_DOWN_ONLY", "NUMPAD 5", "LEFT STICK"),
			new ButtonData(113, "INPUT_VEH_FLY_UNDERCARRIAGE", "G", "L3"),
			new ButtonData(114, "INPUT_VEH_FLY_ATTACK", "RIGHT MOUSE", "BUTTON A"),
			new ButtonData(115, "INPUT_VEH_FLY_SELECT_NEXT_WEAPON", "SCROLLWHEEL UP", "DPAD LEFT"),
			new ButtonData(116, "INPUT_VEH_FLY_SELECT_PREV_WEAPON", "[", "(NONE)"),
			new ButtonData(117, "INPUT_VEH_FLY_SELECT_TARGET_LEFT", "NUMPAD", "7 LB"),
			new ButtonData(118, "INPUT_VEH_FLY_SELECT_TARGET_RIGHT", "NUMPAD", "9 RB"),
			new ButtonData(119, "INPUT_VEH_FLY_VERTICAL_FLIGHT_MODE", "E", "DPAD RIGHT"),
			new ButtonData(120, "INPUT_VEH_FLY_DUCK", "X", "A"),
			new ButtonData(121, "INPUT_VEH_FLY_ATTACK_CAMERA", "INSERT", "R3"),
			new ButtonData(122, "INPUT_VEH_FLY_MOUSE_CONTROL_OVERRIDE", "LEFT", "MOUSE BUTTON(NONE)"),
			new ButtonData(123, "INPUT_VEH_SUB_TURN_LR", "NUMPAD 6", "LEFT STICK"),
			new ButtonData(124, "INPUT_VEH_SUB_TURN_LEFT_ONLY", "NUMPAD 4", "LEFT STICK"),
			new ButtonData(125, "INPUT_VEH_SUB_TURN_RIGHT_ONLY", "NUMPAD 6", "LEFT STICK"),
			new ButtonData(126, "INPUT_VEH_SUB_PITCH_UD", "NUMPAD 5", "LEFT STICK"),
			new ButtonData(127, "INPUT_VEH_SUB_PITCH_UP_ONLY", "NUMPAD 8", "LEFT STICK"),
			new ButtonData(128, "INPUT_VEH_SUB_PITCH_DOWN_ONLY", "NUMPAD 5", "LEFT STICK"),
			new ButtonData(129, "INPUT_VEH_SUB_THROTTLE_UP", "W", "RT"),
			new ButtonData(130, "INPUT_VEH_SUB_THROTTLE_DOWN", "S", "LT"),
			new ButtonData(131, "INPUT_VEH_SUB_ASCEND", "LEFT", "SHIFT X"),
			new ButtonData(132, "INPUT_VEH_SUB_DESCEND", "LEFT", "CTRL A"),
			new ButtonData(133, "INPUT_VEH_SUB_TURN_HARD_LEFT", "A", "LB"),
			new ButtonData(134, "INPUT_VEH_SUB_TURN_HARD_RIGHT", "D", "RB"),
			new ButtonData(135, "INPUT_VEH_SUB_MOUSE_CONTROL_OVERRIDE", "LEFT", "MOUSE BUTTON(NONE)"),
			new ButtonData(136, "INPUT_VEH_PUSHBIKE_PEDAL", "W", "A"),
			new ButtonData(137, "INPUT_VEH_PUSHBIKE_SPRINT", "CAPSLOCK", "A"),
			new ButtonData(138, "INPUT_VEH_PUSHBIKE_FRONT_BRAKE", "Q", "LT"),
			new ButtonData(139, "INPUT_VEH_PUSHBIKE_REAR_BRAKE", "S", "RT"),
			new ButtonData(140, "INPUT_MELEE_ATTACK_LIGHT", "R", "B"),
			new ButtonData(141, "INPUT_MELEE_ATTACK_HEAVY", "Q", "A"),
			new ButtonData(142, "INPUT_MELEE_ATTACK_ALTERNATE", "LEFT MOUSE", "BUTTON RT"),
			new ButtonData(143, "INPUT_MELEE_BLOCK", "SPACEBAR", "X"),
			new ButtonData(144, "INPUT_PARACHUTE_DEPLOY", "F / LEFT MOUSE BUTTON", "Y"),
			new ButtonData(145, "INPUT_PARACHUTE_DETACH", "F", "Y"),
			new ButtonData(146, "INPUT_PARACHUTE_TURN_LR", "D", "LEFT STICK"),
			new ButtonData(147, "INPUT_PARACHUTE_TURN_LEFT_ONLY", "A", "LEFT STICK"),
			new ButtonData(148, "INPUT_PARACHUTE_TURN_RIGHT_ONLY", "D", "LEFT STICK"),
			new ButtonData(149, "INPUT_PARACHUTE_PITCH_UD", "S", "LEFT STICK"),
			new ButtonData(150, "INPUT_PARACHUTE_PITCH_UP_ONLY", "W", "LEFT STICK"),
			new ButtonData(151, "INPUT_PARACHUTE_PITCH_DOWN_ONLY", "S", "LEFT STICK"),
			new ButtonData(152, "INPUT_PARACHUTE_BRAKE_LEFT", "Q", "LB"),
			new ButtonData(153, "INPUT_PARACHUTE_BRAKE_RIGHT", "E", "RB"),
			new ButtonData(154, "INPUT_PARACHUTE_SMOKE", "X", "A"),
			new ButtonData(155, "INPUT_PARACHUTE_PRECISION_LANDING", "LEFT", "SHIFT(NONE)"),
			new ButtonData(156, "INPUT_MAP", "(NONE)", "(NONE)"),
			new ButtonData(157, "INPUT_SELECT_WEAPON_UNARMED", "1", "(NONE)"),
			new ButtonData(158, "INPUT_SELECT_WEAPON_MELEE", "2", "(NONE)"),
			new ButtonData(159, "INPUT_SELECT_WEAPON_HANDGUN", "6", "(NONE)"),
			new ButtonData(160, "INPUT_SELECT_WEAPON_SHOTGUN", "3", "(NONE)"),
			new ButtonData(161, "INPUT_SELECT_WEAPON_SMG", "7", "(NONE)"),
			new ButtonData(162, "INPUT_SELECT_WEAPON_AUTO_RIFLE", "8", "(NONE)"),
			new ButtonData(163, "INPUT_SELECT_WEAPON_SNIPER", "9", "(NONE)"),
			new ButtonData(164, "INPUT_SELECT_WEAPON_HEAVY", "4", "(NONE)"),
			new ButtonData(165, "INPUT_SELECT_WEAPON_SPECIAL", "5", "(NONE)"),
			new ButtonData(166, "INPUT_SELECT_CHARACTER_MICHAEL", "F5", "(NONE)"),
			new ButtonData(167, "INPUT_SELECT_CHARACTER_FRANKLIN", "F6", "(NONE)"),
			new ButtonData(168, "INPUT_SELECT_CHARACTER_TREVOR", "F7", "(NONE)"),
			new ButtonData(169, "INPUT_SELECT_CHARACTER_MULTIPLAYER", "F8(CONSOLE)", "(NONE)"),
			new ButtonData(170, "INPUT_SAVE_REPLAY_CLIP", "F3", "B"),
			new ButtonData(171, "INPUT_SPECIAL_ABILITY_PC", "CAPSLOCK", "(NONE)"),
			new ButtonData(172, "INPUT_CELLPHONE_UP", "ARROW UP", "DPAD UP"),
			new ButtonData(173, "INPUT_CELLPHONE_DOWN", "ARROW DOWN", "DPAD DOWN"),
			new ButtonData(174, "INPUT_CELLPHONE_LEFT", "ARROW LEFT", "DPAD LEFT"),
			new ButtonData(175, "INPUT_CELLPHONE_RIGHT", "ARROW RIGHT", "DPAD RIGHT"),
			new ButtonData(176, "INPUT_CELLPHONE_SELECT", "ENTER / LEFT MOUSE BUTTON", "A"),
			new ButtonData(177, "INPUT_CELLPHONE_CANCEL", "BACKSPACE / ESC / RIGHTMOUSEBUTTON", "B"),
			new ButtonData(178, "INPUT_CELLPHONE_OPTION", "DELETE", "Y"),
			new ButtonData(179, "INPUT_CELLPHONE_EXTRA_OPTION", "SPACEBAR", "X"),
			new ButtonData(180, "INPUT_CELLPHONE_SCROLL_FORWARD", "SCROLLWHEEL", "DOWN(NONE)"),
			new ButtonData(181, "INPUT_CELLPHONE_SCROLL_BACKWARD", "SCROLLWHEEL", "UP(NONE)"),
			new ButtonData(182, "INPUT_CELLPHONE_CAMERA_FOCUS_LOCK", "L", "RT"),
			new ButtonData(183, "INPUT_CELLPHONE_CAMERA_GRID", "G", "RB"),
			new ButtonData(184, "INPUT_CELLPHONE_CAMERA_SELFIE", "E", "R3"),
			new ButtonData(185, "INPUT_CELLPHONE_CAMERA_DOF", "F", "LB"),
			new ButtonData(186, "INPUT_CELLPHONE_CAMERA_EXPRESSION", "X", "L3"),
			new ButtonData(187, "INPUT_FRONTEND_DOWN", "ARROW DOWN", "DPAD DOWN"),
			new ButtonData(188, "INPUT_FRONTEND_UP", "ARROW UP", "DPAD UP"),
			new ButtonData(189, "INPUT_FRONTEND_LEFT", "ARROW LEFT", "DPAD LEFT"),
			new ButtonData(190, "INPUT_FRONTEND_RIGHT", "ARROW RIGHT", "DPAD RIGHT"),
			new ButtonData(191, "INPUT_FRONTEND_RDOWN", "ENTER", "A"),
			new ButtonData(192, "INPUT_FRONTEND_RUP", "TAB", "Y"),
			new ButtonData(193, "INPUT_FRONTEND_RLEFT", "(NONE)", "X"),
			new ButtonData(194, "INPUT_FRONTEND_RRIGHT", "BACKSPACE", "B"),
			new ButtonData(195, "INPUT_FRONTEND_AXIS_X", "D", "LEFT STICK"),
			new ButtonData(196, "INPUT_FRONTEND_AXIS_Y", "S", "LEFT STICK"),
			new ButtonData(197, "INPUT_FRONTEND_RIGHT_AXIS_X]", "RIGHT", "STICK"),
			new ButtonData(198, "INPUT_FRONTEND_RIGHT_AXIS_Y", "SCROLLWHEEL DOWN", "RIGHT STICK"),
			new ButtonData(199, "INPUT_FRONTEND_PAUSE", "P", "START"),
			new ButtonData(200, "INPUT_FRONTEND_PAUSE_ALTERNATE", "ESC", "(NONE)"),
			new ButtonData(201, "INPUT_FRONTEND_ACCEPT", "ENTER / NUMPAD ENTER", "A"),
			new ButtonData(202, "INPUT_FRONTEND_CANCEL", "BACKSPACE /", "ESC B"),
			new ButtonData(203, "INPUT_FRONTEND_X", "SPACEBAR", "X"),
			new ButtonData(204, "INPUT_FRONTEND_Y", "TAB", "Y"),
			new ButtonData(205, "INPUT_FRONTEND_LB", "Q", "LB"),
			new ButtonData(206, "INPUT_FRONTEND_RB", "E", "RB"),
			new ButtonData(207, "INPUT_FRONTEND_LT", "PAGE", "DOWN LT"),
			new ButtonData(208, "INPUT_FRONTEND_RT", "PAGE", "UP RT"),
			new ButtonData(209, "INPUT_FRONTEND_LS", "LEFT", "SHIFT L3"),
			new ButtonData(210, "INPUT_FRONTEND_RS", "LEFT", "CONTROL R3"),
			new ButtonData(211, "INPUT_FRONTEND_LEADERBOARD", "TAB", "RB"),
			new ButtonData(212, "INPUT_FRONTEND_SOCIAL_CLUB", "HOME", "BACK"),
			new ButtonData(213, "INPUT_FRONTEND_SOCIAL_CLUB_SECONDARY", "HOME", "RB"),
			new ButtonData(214, "INPUT_FRONTEND_DELETE", "DELETE", "X"),
			new ButtonData(215, "INPUT_FRONTEND_ENDSCREEN_ACCEPT", "ENTER", "A"),
			new ButtonData(216, "INPUT_FRONTEND_ENDSCREEN_EXPAND", "SPACEBAR", "X"),
			new ButtonData(217, "INPUT_FRONTEND_SELECT", "CAPSLOCK", "BACK"),
			new ButtonData(218, "INPUT_SCRIPT_LEFT_AXIS_X", "D", "LEFT STICK"),
			new ButtonData(219, "INPUT_SCRIPT_LEFT_AXIS_Y", "S", "LEFT STICK"),
			new ButtonData(220, "INPUT_SCRIPT_RIGHT_AXIS_X", "MOUSE RIGHT", "RIGHT STICK"),
			new ButtonData(221, "INPUT_SCRIPT_RIGHT_AXIS_Y", "MOUSE DOWN", "RIGHT STICK"),
			new ButtonData(222, "INPUT_SCRIPT_RUP", "RIGHT MOUSE", "BUTTON Y"),
			new ButtonData(223, "INPUT_SCRIPT_RDOWN", "LEFT MOUSE", "BUTTON A"),
			new ButtonData(224, "INPUT_SCRIPT_RLEFT", "LEFT", "CTRL X"),
			new ButtonData(225, "INPUT_SCRIPT_RRIGHT", "RIGHT MOUSE", "BUTTON B"),
			new ButtonData(226, "INPUT_SCRIPT_LB", "(NONE)", "LB"),
			new ButtonData(227, "INPUT_SCRIPT_RB", "(NONE)", "LT"),
			new ButtonData(228, "INPUT_SCRIPT_LT", "(NONE)", "LB"),
			new ButtonData(229, "INPUT_SCRIPT_RT", "LEFT MOUSE", "BUTTON RT"),
			new ButtonData(230, "INPUT_SCRIPT_LS", "(NONE)", "L3"),
			new ButtonData(231, "INPUT_SCRIPT_RS", "(NONE)", "R3"),
			new ButtonData(232, "INPUT_SCRIPT_PAD_UP", "W", "DPAD UP"),
			new ButtonData(233, "INPUT_SCRIPT_PAD_DOWN", "S", "DPAD DOWN"),
			new ButtonData(234, "INPUT_SCRIPT_PAD_LEFT", "A", "DPAD LEFT"),
			new ButtonData(235, "INPUT_SCRIPT_PAD_RIGHT", "D", "DPAD RIGHT"),
			new ButtonData(236, "INPUT_SCRIPT_SELECT", "V", "BACK"),
			new ButtonData(237, "INPUT_CURSOR_ACCEPT", "LEFT MOUSE", "BUTTON (NONE)"),
			new ButtonData(238, "INPUT_CURSOR_CANCEL", "RIGHT MOUSE", "BUTTON (NONE)"),
			new ButtonData(239, "INPUT_CURSOR_X", "(NONE)", "(NONE)"),
			new ButtonData(240, "INPUT_CURSOR_Y", "(NONE)", "(NONE)"),
			new ButtonData(241, "INPUT_CURSOR_SCROLL_UP", "SCROLLWHEEL", "UP (NONE)"),
			new ButtonData(242, "INPUT_CURSOR_SCROLL_DOWN", "SCROLLWHEEL", "DOWN (NONE)"),
			new ButtonData(243, "INPUT_ENTER_CHEAT_CODE", "~ /", "` (NONE)"),
			new ButtonData(244, "INPUT_INTERACTION_MENU", "M", "BACK"),
			new ButtonData(245, "INPUT_MP_TEXT_CHAT_ALL", "T", "(NONE)"),
			new ButtonData(246, "INPUT_MP_TEXT_CHAT_TEAM", "Y", "(NONE)"),
			new ButtonData(247, "INPUT_MP_TEXT_CHAT_FRIENDS", "(NONE)", "(NONE)"),
			new ButtonData(248, "INPUT_MP_TEXT_CHAT_CREW", "(NONE)", "(NONE)"),
			new ButtonData(249, "INPUT_PUSH_TO_TALK", "N", "(NONE)"),
			new ButtonData(250, "INPUT_CREATOR_LS", "R", "L3"),
			new ButtonData(251, "INPUT_CREATOR_RS", "F", "R3"),
			new ButtonData(252, "INPUT_CREATOR_LT", "X", "LT"),
			new ButtonData(253, "INPUT_CREATOR_RT", "C", "RT"),
			new ButtonData(254, "INPUT_CREATOR_MENU_TOGGLE", "LEFT", "SHIFT (NONE)"),
			new ButtonData(255, "INPUT_CREATOR_ACCEPT", "SPACEBAR", "A"),
			new ButtonData(256, "INPUT_CREATOR_DELETE", "DELETE", "X"),
			new ButtonData(257, "INPUT_ATTACK2", "LEFT MOUSE", "BUTTON RT"),
			new ButtonData(258, "INPUT_RAPPEL_JUMP", "(NONE)", "A"),
			new ButtonData(259, "INPUT_RAPPEL_LONG_JUMP", "(NONE)", "X"),
			new ButtonData(260, "INPUT_RAPPEL_SMASH_WINDOW", "(NONE)", "RT"),
			new ButtonData(261, "INPUT_PREV_WEAPON", "SCROLLWHEEL UP", "DPAD LEFT"),
			new ButtonData(262, "INPUT_NEXT_WEAPON", "SCROLLWHEEL DOWN", "DPAD RIGHT"),
			new ButtonData(263, "INPUT_MELEE_ATTACK1", "R", "B"),
			new ButtonData(264, "INPUT_MELEE_ATTACK2", "Q", "A"),
			new ButtonData(265, "INPUT_WHISTLE", "(NONE)", "(NONE)"),
			new ButtonData(266, "INPUT_MOVE_LEFT", "D", "LEFT STICK"),
			new ButtonData(267, "INPUT_MOVE_RIGHT", "D", "LEFT STICK"),
			new ButtonData(268, "INPUT_MOVE_UP", "S", "LEFT STICK"),
			new ButtonData(269, "INPUT_MOVE_DOWN", "S", "LEFT STICK"),
			new ButtonData(270, "INPUT_LOOK_LEFT", "MOUSE RIGHT", "RIGHT STICK"),
			new ButtonData(271, "INPUT_LOOK_RIGHT", "MOUSE RIGHT", "RIGHT STICK"),
			new ButtonData(272, "INPUT_LOOK_UP", "MOUSE DOWN", "RIGHT STICK"),
			new ButtonData(273, "INPUT_LOOK_DOWN", "MOUSE DOWN", "RIGHT STICK"),
			new ButtonData(274, "INPUT_SNIPER_ZOOM_IN", "[RIGHT", "STICK"),
			new ButtonData(275, "INPUT_SNIPER_ZOOM_OUT", "[RIGHT", "STICK"),
			new ButtonData(276, "INPUT_SNIPER_ZOOM_IN_ALTERNATE", "[LEFT", "STICK"),
			new ButtonData(277, "INPUT_SNIPER_ZOOM_OUT_ALTERNATE", "[LEFT", "STICK"),
			new ButtonData(278, "INPUT_VEH_MOVE_LEFT", "D", "LEFT STICK"),
			new ButtonData(279, "INPUT_VEH_MOVE_RIGHT", "D", "LEFT STICK"),
			new ButtonData(280, "INPUT_VEH_MOVE_UP", "LEFT CTRL", "LEFT STICK"),
			new ButtonData(281, "INPUT_VEH_MOVE_DOWN", "LEFT CTRL", "LEFT STICK"),
			new ButtonData(282, "INPUT_VEH_GUN_LEFT", "MOUSE RIGHT", "RIGHT STICK"),
			new ButtonData(283, "INPUT_VEH_GUN_RIGHT", "MOUSE RIGHT", "RIGHT STICK"),
			new ButtonData(284, "INPUT_VEH_GUN_UP", "MOUSE RIGHT", "RIGHT STICK"),
			new ButtonData(285, "INPUT_VEH_GUN_DOWN", "MOUSE RIGHT", "RIGHT STICK"),
			new ButtonData(286, "INPUT_VEH_LOOK_LEFT", "MOUSE RIGHT", "RIGHT STICK"),
			new ButtonData(287, "INPUT_VEH_LOOK_RIGHT", "MOUSE RIGHT", "RIGHT STICK"),
			new ButtonData(288, "INPUT_REPLAY_START_STOP_RECORDING", "F1", "A"),
			new ButtonData(289, "INPUT_REPLAY_START_STOP_RECORDING_SECONDARY", "F2", "X"),
			new ButtonData(290, "INPUT_SCALED_LOOK_LR", "MOUSE RIGHT", "RIGHT STICK"),
			new ButtonData(291, "INPUT_SCALED_LOOK_UD", "MOUSE DOWN", "RIGHT STICK"),
			new ButtonData(292, "INPUT_SCALED_LOOK_UP_ONLY(NONE)", "RIGHT", "STICK"),
			new ButtonData(293, "INPUT_SCALED_LOOK_DOWN_ONLY(NONE)", "RIGHT", "STICK"),
			new ButtonData(294, "INPUT_SCALED_LOOK_LEFT_ONLY(NONE)", "RIGHT", "STICK"),
			new ButtonData(295, "INPUT_SCALED_LOOK_RIGHT_ONLY(NONE)", "RIGHT", "STICK"),
			new ButtonData(296, "INPUT_REPLAY_MARKER_DELETE", "DELETE", "X"),
			new ButtonData(297, "INPUT_REPLAY_CLIP_DELETE", "DELETE", "Y"),
			new ButtonData(298, "INPUT_REPLAY_PAUSE", "SPACEBAR", "A"),
			new ButtonData(299, "INPUT_REPLAY_REWIND", "ARROW", "DOWN LB"),
			new ButtonData(300, "INPUT_REPLAY_FFWD", "ARROW", "UP RB"),
			new ButtonData(301, "INPUT_REPLAY_NEWMARKER", "M", "A"),
			new ButtonData(303, "INPUT_REPLAY_SCREENSHOT", "U", "DPAD UP"),
			new ButtonData(304, "INPUT_REPLAY_HIDEHUD", "H", "R3"),
			new ButtonData(307, "INPUT_REPLAY_ADVANCE", "ARROW RIGHT", "DPAD RIGHT"),
			new ButtonData(308, "INPUT_REPLAY_BACK", "ARROW LEFT", "DPAD LEFT"),
			new ButtonData(309, "INPUT_REPLAY_TOOLS", "T", "DPAD DOWN"),
			new ButtonData(310, "INPUT_REPLAY_RESTART", "R", "BACK"),
			new ButtonData(311, "INPUT_REPLAY_SHOWHOTKEY", "K", "DPAD DOWN"),
			new ButtonData(312, "INPUT_REPLAY_CYCLEMARKERLEFT", "[DPAD", "LEFT"),
			new ButtonData(313, "INPUT_REPLAY_CYCLEMARKERRIGHT]", "DPAD", "RIGHT"),
			new ButtonData(314, "INPUT_REPLAY_FOVINCREASE", "NUMPAD", "+ RB"),
			new ButtonData(315, "INPUT_REPLAY_FOVDECREASE", "NUMPAD", "- LB"),
			new ButtonData(316, "INPUT_REPLAY_CAMERAUP", "PAGE", "UP (NONE)"),
			new ButtonData(317, "INPUT_REPLAY_CAMERADOWN", "PAGE", "DOWN (NONE)"),
			new ButtonData(318, "INPUT_REPLAY_SAVE", "F5", "START"),
			new ButtonData(319, "INPUT_REPLAY_TOGGLETIME", "C", "(NONE)"),
			new ButtonData(320, "INPUT_REPLAY_TOGGLETIPS", "V", "(NONE)"),
			new ButtonData(321, "INPUT_REPLAY_PREVIEW", "SPACEBAR", "(NONE)"),
			new ButtonData(322, "INPUT_REPLAY_TOGGLE_TIMELINE", "ESC", "(NONE)"),
			new ButtonData(323, "INPUT_REPLAY_TIMELINE_PICKUP_CLIP", "X", "(NONE)"),
			new ButtonData(324, "INPUT_REPLAY_TIMELINE_DUPLICATE_CLIP", "C", "(NONE)"),
			new ButtonData(325, "INPUT_REPLAY_TIMELINE_PLACE_CLIP", "V", "(NONE)"),
			new ButtonData(326, "INPUT_REPLAY_CTRL", "LEFT CTRL", "(NONE)"),
			new ButtonData(327, "INPUT_REPLAY_TIMELINE_SAVE", "F5", "CTRL (NONE)"),
			new ButtonData(328, "INPUT_REPLAY_PREVIEW_AUDIO", "SPACEBAR", "RT"),
			new ButtonData(329, "INPUT_VEH_DRIVE_LOOK", "LEFT MOUSE", "BUTTON (NONE)"),
			new ButtonData(330, "INPUT_VEH_DRIVE_LOOK2", "RIGHT MOUSE", "BUTTON (NONE)"),
			new ButtonData(331, "INPUT_VEH_FLY_ATTACK2", "RIGHT MOUSE", "BUTTON (NONE)"),
			new ButtonData(332, "INPUT_RADIO_WHEEL_UD", "MOUSE DOWN", "RIGHT STICK"),
			new ButtonData(333, "INPUT_RADIO_WHEEL_LR", "MOUSE RIGHT", "RIGHT STICK"),
			new ButtonData(334, "INPUT_VEH_SLOWMO_UD", "SCROLLWHEEL DOWN", "LEFT STICK"),
			new ButtonData(335, "INPUT_VEH_SLOWMO_UP_ONLY", "SCROLLWHEEL UP", "LEFT STICK"),
			new ButtonData(336, "INPUT_VEH_SLOWMO_DOWN_ONLY", "SCROLLWHEEL DOWN", "LEFT STICK"),
			new ButtonData(337, "INPUT_VEH_HYDRAULICS_CONTROL_TOGGLE", "X", "A"),
			new ButtonData(338, "INPUT_VEH_HYDRAULICS_CONTROL_LEFT", "A", "LEFT STICK"),
			new ButtonData(339, "INPUT_VEH_HYDRAULICS_CONTROL_RIGHT", "D", "LEFT STICK"),
			new ButtonData(340, "INPUT_VEH_HYDRAULICS_CONTROL_UP", "LEFT SHIFT", "LEFT STICK"),
			new ButtonData(341, "INPUT_VEH_HYDRAULICS_CONTROL_DOWN", "LEFT CTRL", "LEFT STICK"),
			new ButtonData(342, "INPUT_VEH_HYDRAULICS_CONTROL_UD", "D", "LEFT STICK"),
			new ButtonData(343, "INPUT_VEH_HYDRAULICS_CONTROL_LR", "LEFT CTRL", "LEFT STICK"),
			new ButtonData(344, "INPUT_SWITCH_VISOR", "F11", "DPAD RIGHT"),
			new ButtonData(345, "INPUT_VEH_MELEE_HOLD", "X", "A"),
			new ButtonData(346, "INPUT_VEH_MELEE_LEFT", "LEFT MOUSE", "BUTTON LB"),
			new ButtonData(347, "INPUT_VEH_MELEE_RIGHT", "RIGHT MOUSE", "BUTTON RB"),
			new ButtonData(348, "INPUT_MAP_POI", "SCROLLWHEEL BUTTON", "(PRESS) Y"),
			new ButtonData(349, "INPUT_REPLAY_SNAPMATIC_PHOTO", "TAB", "X"),
			new ButtonData(350, "INPUT_VEH_CAR_JUMP", "E", "L3"),
			new ButtonData(351, "INPUT_VEH_ROCKET_BOOST", "E", "L3"),
			new ButtonData(352, "INPUT_VEH_FLY_BOOST", "LEFT", "SHIFT L3"),
			new ButtonData(353, "INPUT_VEH_PARACHUTE", "SPACEBAR", "A"),
			new ButtonData(354, "INPUT_VEH_BIKE_WINGS", "X", "A"),
			new ButtonData(355, "INPUT_VEH_FLY_BOMB_BAY", "E", "DPAD RIGHT"),
			new ButtonData(356, "INPUT_VEH_FLY_COUNTER", "E", "DPAD RIGHT"),
			new ButtonData(357, "INPUT_VEH_TRANSFORM", "X", "A")
		};

		public Vector2 ScreenPos = new Vector2(0.125f, 0.125f);

		public Vector2 ScreenSize = new Vector2(0.25f, 0.25f);

		public int FinaleSelection_Approach = 64;

		public int FinaleSelection_Infiltration = 51;

		public int FinaleSelection_CompoundEntryPoint = 85;

		public int FinaleSelection_EscapePoint = 87;

		public int FinaleSelection_TimeOfDay = 91;

		public int CreatedSetupMenus = -1;

		public int f_1362;

		public int f_1363;

		public int Local_348;

		public int f_1375;

		public string f_1374;

		public string f_1373;

		public int SwitchStage = 11;

		public bool DrawTable;

		public int OStage;

		public int OSF1 = -1;

		public int OSF2 = -1;

		public int OSFScreen = 1;

		public int OSFSelection = 1;

		public int OverlaySelectedIndex;

		public int GUIAPP;

		public int GUIBIX;

		public int LastGUIBix;

		public bool GUICheckBool;

		public int SelectedMCBusiness;

		public int SelectedScreenID;

		public int SelectedRollover;

		public int SelectedSelection;

		public int SelectedBus;

		public int SelectedRoll;

		public int SelectedScr;

		public int SelectedSel;

		public static bool TableDebug;

		public static bool InteractingWithCayoTableScreen;

		private Prop TableScreen;

		private Prop TableScreen2;

		private Prop Egg;

		public static Camera ScreenCamera;

		public static bool BeenLoading;

		public int SlotsUsed = 1;

		public static string LastInputtedCommand;

		public static int InputtedCommandStage;

		public static bool IsPlayerInKosatka_SPH()
		{
			return false;
		}

		public static bool IsPlayerInKosatka_Standalone()
		{
			return false;
		}

		public static int GetSubTableScaleformHandle()
		{
			int result = 0;
			if (UseScaleformVarInsteadOfInt)
			{
				result = SubTableScale.Handle;
			}
			return result;
		}

		public static string HEIST_ISLAND_GET_RENDERTARGET_NAME()
		{
			return "submarine_table";
		}

		public static int HEIST_ISLAND_GET_RENDERTARGET_MODEL()
		{
			return Function.Call<int>(Hash.GET_HASH_KEY, "h4_Int_Sub_Command_Table");
		}

		public static string HEIST_ISLAND_SCALEFORM_GET_MOVIE()
		{
			return "ISLAND_HEIST_BOARD";
		}

		public float HeistIslandScaleformGetCentreX()
		{
			return 0.25f;
		}

		public float HeistIslandScaleformGetCentreY()
		{
			return 0.75f;
		}

		public float HeistIslandScaleformGetWidth()
		{
			return 0.5f;
		}

		public float HeistIslandScaleformGetHeight()
		{
			return 0.5f;
		}

		private void func_CayoMap4448(string sParam0, string sParam1)
		{
			f_1374 = sParam0;
			f_1373 = sParam1;
		}

		private string func_CayoMap4445(int I)
		{
			switch (I)
			{
			case 1:
				return "2_BOOTUP";
			case 2:
				return "3_BOOTUP";
			case 3:
				return "4_BOOTUP";
			case 4:
				return "EL_RUBIO_1";
			case 5:
				return "EL_RUBIO_2";
			case 6:
				return "EL_RUBIO_3";
			case 7:
				return "EL_RUBIO_4";
			case 8:
				return "EL_RUBIO_5";
			case 9:
				return "EL_RUBIO_6";
			case 10:
				return "1_BOOTUP";
			case 11:
				return "EL_RUBIO";
			case 12:
				return "ISLAND";
			case 13:
				return "OPERATIONS";
			default:
				return "";
			}
		}

		private string func_CayoMap4446(int I)
		{
			switch (I)
			{
			case 1:
				return "HS4_SUB_INT_BOOT";
			case 2:
				return "HS4_SUB_INT_BOOT_2";
			case 3:
				return "HS4_SUB_INT_BOOT_2";
			case 4:
				return "HS4_SUB_INT_EL_RUBIO_1";
			case 5:
				return "HS4_SUB_INT_EL_RUBIO_2";
			case 6:
				return "HS4_SUB_INT_EL_RUBIO_3";
			case 7:
				return "HS4_SUB_INT_EL_RUBIO_4";
			case 8:
				return "HS4_SUB_INT_EL_RUBIO_5";
			case 9:
				return "HS4_SUB_INT_EL_RUBIO_6";
			case 10:
				return "HS4_SUB_INT_BOOT";
			case 11:
				return "HS4_SUB_INT_EL_RUBIO";
			case 12:
				return "HS4_SUB_INT";
			case 13:
				return "HS4_SUB_INT_EL_RUBIO";
			default:
				return "";
			}
		}

		private void func_CayoMap4443()
		{
			switch (f_1362)
			{
			case 0:
				Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "HS4_SUB_INT", false);
				Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "HS4_SUB_INT_BOOT", false);
				Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "HS4_SUB_INT_BOOT_2", false);
				Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "HS4_SUB_INT_EL_RUBIO", false);
				Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "HS4_SUB_INT_EL_RUBIO_1", false);
				Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "HS4_SUB_INT_EL_RUBIO_2", false);
				Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "HS4_SUB_INT_EL_RUBIO_3", false);
				Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "HS4_SUB_INT_EL_RUBIO_4", false);
				Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "HS4_SUB_INT_EL_RUBIO_5", false);
				Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "HS4_SUB_INT_EL_RUBIO_6", false);
				if (Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, "HS4_SUB_INT_BOOT"))
				{
					f_1362 = 1;
				}
				break;
			case 1:
				if (!Function.Call<bool>(Hash.IS_NAMED_RENDERTARGET_REGISTERED, "submarine_table"))
				{
					Function.Call(Hash.REGISTER_NAMED_RENDERTARGET, "submarine_table", false);
					if (!Function.Call<bool>(Hash.IS_NAMED_RENDERTARGET_LINKED, 2461393867u))
					{
						Function.Call(Hash.LINK_NAMED_RENDERTARGET, 2461393867u);
						if (f_1363 == -1)
						{
							f_1363 = Function.Call<int>(Hash.GET_NAMED_RENDERTARGET_RENDER_ID, "submarine_table");
							f_1362 = 2;
						}
					}
				}
				if (Function.Call<bool>(Hash.IS_NAMED_RENDERTARGET_REGISTERED, "submarine_table"))
				{
					break;
				}
				Function.Call(Hash.REGISTER_NAMED_RENDERTARGET, "submarine_table", false);
				if (!Function.Call<bool>(Hash.IS_NAMED_RENDERTARGET_LINKED, Function.Call<int>(Hash.GET_HASH_KEY, "ba_prop_battle_hacker_screen")))
				{
					Function.Call(Hash.LINK_NAMED_RENDERTARGET, Function.Call<int>(Hash.GET_HASH_KEY, "ba_prop_battle_hacker_screen"));
					if (f_1363 == -1)
					{
						f_1363 = Function.Call<int>(Hash.GET_NAMED_RENDERTARGET_RENDER_ID, "submarine_table");
					}
				}
				break;
			case 2:
				Function.Call(Hash.SET_TEXT_RENDER_ID, f_1363);
				Function.Call(Hash.SET_SCRIPT_GFX_ALIGN, 73, 73);
				Function.Call(Hash.SET_SCRIPT_GFX_DRAW_ORDER, 4);
				Function.Call(Hash.SET_SCRIPT_GFX_DRAW_BEHIND_PAUSEMENU, true);
				Function.Call(Hash.DRAW_SPRITE_NAMED_RENDERTARGET, f_1374, f_1373, 0.5f, 0.5f, 1f, 1f, 0f, 255, 255, 255, 255);
				Function.Call(Hash.SET_SCRIPT_GFX_ALIGN);
				Function.Call(Hash.SET_TEXT_RENDER_ID, Function.Call<int>(Hash.GET_DEFAULT_SCRIPT_RENDERTARGET_RENDER_ID));
				break;
			}
		}

		private void func_CayoMap4451()
		{
			f_1363 = -1;
			f_1362 = 0;
			if (Function.Call<bool>(Hash.IS_NAMED_RENDERTARGET_REGISTERED, "submarine_table"))
			{
				Function.Call(Hash.RELEASE_NAMED_RENDERTARGET, "submarine_table");
			}
			Function.Call(Hash.SET_STREAMED_TEXTURE_DICT_AS_NO_LONGER_NEEDED, "HS4_SUB_INT");
			Function.Call(Hash.SET_STREAMED_TEXTURE_DICT_AS_NO_LONGER_NEEDED, "HS4_SUB_INT_BOOT");
			Function.Call(Hash.SET_STREAMED_TEXTURE_DICT_AS_NO_LONGER_NEEDED, "HS4_SUB_INT_BOOT_2");
			Function.Call(Hash.SET_STREAMED_TEXTURE_DICT_AS_NO_LONGER_NEEDED, "HS4_SUB_INT_EL_RUBIO");
			Function.Call(Hash.SET_STREAMED_TEXTURE_DICT_AS_NO_LONGER_NEEDED, "HS4_SUB_INT_EL_RUBIO_1");
			Function.Call(Hash.SET_STREAMED_TEXTURE_DICT_AS_NO_LONGER_NEEDED, "HS4_SUB_INT_EL_RUBIO_2");
			Function.Call(Hash.SET_STREAMED_TEXTURE_DICT_AS_NO_LONGER_NEEDED, "HS4_SUB_INT_EL_RUBIO_3");
			Function.Call(Hash.SET_STREAMED_TEXTURE_DICT_AS_NO_LONGER_NEEDED, "HS4_SUB_INT_EL_RUBIO_4");
			Function.Call(Hash.SET_STREAMED_TEXTURE_DICT_AS_NO_LONGER_NEEDED, "HS4_SUB_INT_EL_RUBIO_5");
			Function.Call(Hash.SET_STREAMED_TEXTURE_DICT_AS_NO_LONGER_NEEDED, "HS4_SUB_INT_EL_RUBIO_6");
		}

		private void func_34(int iParam1)
		{
			string text = "";
			string text2 = "";
			text = func_37(iParam1);
			text2 = func_36();
			Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, text, text2, true);
		}

		private string func_36()
		{
			return "DLC_HEI4_PLANNING_BOARD_PLANNING_BOARD_SOUNDSET";
		}

		private string func_37(int iParam0)
		{
			switch (iParam0)
			{
			case 0:
				return "NAV_UP_DOWN";
			case 1:
				return "NAV_LEFT_RIGHT";
			case 2:
				return "NAV_UP_DOWN_PHOTO_CHANGE";
			case 3:
				return "NAV_LEFT_RIGHT_PHOTO_CHANGE";
			case 4:
				return "NAV_BLOCKED";
			case 201:
				return "SELECT";
			case 202:
				return "BACK";
			case 7:
				return "ERROR";
			case 8:
				return "DRAW_BOARD";
			case 9:
				return "DRAW_DESCRIPTION_TEXT";
			case 10:
				return "DRAW_TITLE_TEXT";
			case 11:
				return "PAY";
			case 12:
				return "INCREASE_LOOT_SHARE";
			case 13:
				return "DECREASE_LOOT_SHARE";
			case 14:
				return "USE";
			case 15:
				return "LAUNCH_MISSION";
			case 16:
				return "REMOTE_READY";
			case 17:
				return "REMOTE_UNREADY";
			case 18:
				return "BOOTUP";
			case 19:
				return "BACKGROUND";
			case 20:
				return "FINALE_AVAILABLE";
			case 21:
				return "CHANGE_DISPLAY_LARGE_BOX_DRAW";
			case 22:
				return "CHANGE_DISPLAY_SMALL_BOX_DRAW";
			default:
				return "";
			}
		}

		private string func_347(int iParam1)
		{
			switch (iParam1)
			{
			case 1:
				return "H4P_INT0_GATH_I";
			case 2:
				return "H4P_INT0_LOOT_I";
			case 3:
				return "H4P_INT0_ENTR_I";
			case 4:
				return "H4P_INT0_EXIT_I";
			case 5:
				return "H4P_INT0_COMP_I";
			case 6:
				return "H4P_INT0_ITEM_I";
			case 7:
				return "H4P_INT2_CASH_I";
			case 8:
				return "H4P_INT2_WEED_I";
			case 9:
				return "H4P_INT2_COKE_I";
			case 10:
				return "H4P_INT2_GOLD_I";
			case 11:
				return "H4P_INT2_PAIN_I";
			case 12:
				return "H4P_INT3_AIRS_I";
			case 13:
				return "H4P_INT3_PARA_I";
			case 14:
				return "H4P_INT3_BEAC_I";
			case 15:
				return "H4P_INT3_MDCK_I";
			case 16:
				return "H4P_INT3_NDCK_I";
			case 17:
				return "H4P_INT3_NDRP_I";
			case 18:
				return "H4P_INT3_SDRP_I";
			case 19:
				return "H4P_INT3_DTUN_I";
			case 20:
				return "H4P_INT4_AIRS_I";
			case 21:
				return "H4P_INT4_MDCK_I";
			case 22:
				return "H4P_INT4_NDCK_I";
			case 23:
				return "H4P_INT4_SUBM_I";
			case 30:
				return "H4P_INT6_PWRS_I";
			case 31:
				return "H4P_INT6_CTOW_I";
			case 32:
				return "H4P_INT6_BCUT_I";
			case 33:
				return "H4P_INT6_GRAP_I";
			case 34:
				return "H4P_INT6_UNIF_I";
			case 35:
				return "H4P_INT6_TROJ_I";
			case 24:
				return "H4P_INT5_MGAT_I";
			case 25:
				return "H4P_INT5_NWAL_I";
			case 26:
				return "H4P_INT5_NSGT_I";
			case 27:
				return "H4P_INT5_SWAL_I";
			case 28:
				return "H4P_INT5_SSGT_I";
			case 29:
				return "H4P_INT5_DTUN_I";
			case 36:
				return "";
			case 37:
				return "";
			case 38:
				return "";
			case 39:
				return "";
			case 40:
				return "H4P_PRP1_SUBM_I";
			case 41:
				return "";
			case 42:
				return "H4P_PRP1_SPLA_I";
			case 43:
				return "";
			case 44:
				return "H4P_PRP1_PBOA_I";
			case 45:
				return "H4P_PRP1_SBOA_I";
			case 46:
				return "H4P_PRP1_BOM1_I";
			case 47:
				return "H4P_PRP1_BOM2_I";
			case 48:
				return "H4P_PRP1_HEL1_I";
			case 49:
				return "H4P_PRP1_HEL2_I";
			case 50:
				return "H4P_PRP2_DEMC_I";
			case 53:
				return "H4P_PRP2_ATOR_I";
			case 51:
				return "H4P_PRP2_TAR1_I";
			case 52:
				return "H4P_PRP2_FING_I";
			case 54:
				return "H4P_PRP3_SHOT_I";
			case 55:
				return "H4P_PRP3_RIFL_I";
			case 56:
				return "H4P_PRP3_SNIP_I";
			case 57:
				return "H4P_PRP3_M2SM_2";
			case 58:
				return "H4P_PRP3_M2RI_2";
			case 59:
				return "H4P_PRP3_SUPP_I";
			case 60:
				return "H4P_PRP4_WEAP_I";
			case 61:
				return "H4P_PRP4_ARMR_I";
			case 62:
				return "H4P_PRP4_BRES_I";
			case 63:
				return "";
			case 64:
				return "";
			case 65:
				return "";
			case 66:
				return "";
			case 67:
				return "";
			case 68:
				return "";
			case 69:
				return "";
			case 70:
				return "";
			case 71:
				return "H4P_FIN1_SUBM_I";
			case 72:
				return "H4P_FIN1_BOMB_I";
			case 73:
				return "H4P_FIN1_SMPL_I";
			case 74:
				return "H4P_FIN1_SHEL_I";
			case 75:
				return "H4P_FIN1_PBOA_I";
			case 76:
				return "H4P_FIN1_SBOA_I";
			case 77:
				return "H4P_FIN2_AIRS_I";
			case 78:
				return "H4P_FIN2_PARA_I";
			case 79:
				return "H4P_FIN2_WBEA_I";
			case 80:
				return "H4P_FIN2_MDOC_I";
			case 81:
				return "H4P_FIN2_NDOC_I";
			case 82:
				return "H4P_FIN2_NDRP_I";
			case 83:
				return "H4P_FIN2_SDRP_I";
			case 84:
				return "H4P_FIN2_DTUN_I";
			case 85:
				return "H4P_FIN3_MGAT_I";
			case 86:
				return "H4P_FIN3_DTUN_I";
			case 87:
				return "H4P_FIN4_AIRS_I";
			case 88:
				return "H4P_FIN4_MDOC_I";
			case 89:
				return "H4P_FIN4_NDOC_I";
			case 90:
				return "H4P_FIN4_SUBM_I";
			case 91:
				return "H4P_FIN5_TDAY_I";
			case 92:
				return "H4P_FIN5_TNGT_I";
			case 93:
				return "H4P_FIN6_SHOT_I";
			case 94:
				return "H4P_FIN6_RIFL_I";
			case 95:
				return "H4P_FIN6_SNIP_I";
			case 96:
				return "H4P_FIN6_MKSM_I";
			case 97:
				return "H4P_FIN6_MKAR_I";
			case 98:
				return "H4P_FIN6_SUPP_I";
			case 99:
				return "H4P_FIN7_AIRS_I";
			case 100:
				return "H4P_FIN7_WDRP_I";
			case 101:
				return "H4P_FIN7_SNIP_I";
			case 102:
				return "H4P_FIN7_HELI_I";
			case 103:
				return "H4P_FIN7_SPYD_I";
			case 104:
				return "H4P_FIN7_WEAP_I";
			case 1000:
				return "H4P_PRP2_TAR1_I";
			case 2000:
				return "H4P_PRP2_TAR1_I";
			default:
				return "";
			}
		}

		private string func_388(bool bParam0, string sParam1, string sParam2)
		{
			if (bParam0)
			{
				return sParam1;
			}
			return sParam2;
		}

		private string func_IsSafeTarget(bool bParam0, string sParam1, string sParam2)
		{
			if (bParam0)
			{
				return sParam1;
			}
			return sParam2;
		}

		private void func_413(string sParam0)
		{
			Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, sParam0);
			Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING);
		}

		private void func_524(int iParam0)
		{
			Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD, iParam0, "HIDE_OVERLAY");
		}

		private void func_529(int iParam0)
		{
			Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, iParam0, "REQUEST_CURRENT_ROLLOVER");
			iLocal_1424 = Function.Call<int>(Hash.END_SCALEFORM_MOVIE_METHOD_RETURN_VALUE);
			iLocal_1426 = 1;
		}

		private void func_530(int iParam0, int iParam1)
		{
			Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, iParam0, "SET_INPUT_EVENT", (float)iParam1, -1f, -1f, -1f, -1f);
		}

		private string func_541()
		{
			return "ISLAND_HEIST_BOARD";
		}

		private int func_542()
		{
			return 771658040;
		}

		private string func_543()
		{
			return "submarine_table";
		}

		private void func_547(int iParam0, int iParam1, bool bParam2)
		{
			Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, iParam0, "SET_FINALE_ITEM_SELECTED");
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, iParam1);
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_BOOL, bParam2);
			Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
		}

		private void func_551()
		{
			func_547(GetSubTableScaleformHandle(), 87, false);
			func_547(GetSubTableScaleformHandle(), 88, false);
			func_547(GetSubTableScaleformHandle(), 89, false);
			func_547(GetSubTableScaleformHandle(), 90, false);
		}

		private void func_558(int iParam0, List<dynamic> Param1, bool bParam10, bool Toggle)
		{
			if (Param1.Count != 0)
			{
				Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, iParam0, "ADD_FINALE_ITEM");
				Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, Param1[0]);
				Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, Param1[1]);
				if (bParam10)
				{
					func_559(Param1[2].ToString());
				}
				else
				{
					func_413(Param1[3].ToString());
					Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_BOOL, Toggle);
					func_413(Param1[4].ToString());
					Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_TEXTURE_NAME_STRING, Param1[5]);
					Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, Param1[6]);
					Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_BOOL, Param1[7]);
				}
				Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
				string text = "";
				for (int i = 0; i < Param1.Count; i++)
				{
					dynamic val = Param1[i];
					text = (string)(text + ("(" + i + "): " + val.ToString() + " , "));
				}
			}
		}

		private int func_561(int iParam0)
		{
			if (iParam0 == 59 || (uint)(iParam0 - 99) <= 5u)
			{
				return 1;
			}
			return 0;
		}

		private string func_563(int uParam0, int iParam1)
		{
			switch (iParam1)
			{
			case 1:
				return func_564(uParam0);
			case 2:
				return "ADDITIONAL_LOOT";
			case 3:
				return "INFILTRATION_POINTS";
			case 4:
				return "ESCAPE_POINTS";
			case 5:
				return "COMPOUND_ENTRY_POINTS";
			case 6:
				return "POINTS_OF_INTEREST";
			case 7:
				return "CASH";
			case 8:
				return "WEED";
			case 9:
				return "COCAINE";
			case 10:
				return "GOLD";
			case 11:
				return "PAINTINGS";
			case 12:
				return "AIRSTRIP";
			case 13:
				return "PARACHUTING";
			case 14:
				return "WEST_BEACH";
			case 15:
				return "MAIN_DOCK";
			case 16:
				return "NORTH_DOCK";
			case 17:
				return "NORTH_DROP_ZONE";
			case 18:
				return "SOUTH_DROP_ZONE";
			case 19:
				return "DRAINAGE_TUNNEL";
			case 20:
				return "AIRSTRIP";
			case 21:
				return "MAIN_DOCK";
			case 22:
				return "NORTH_DOCK";
			case 23:
				return "SUBMARINE";
			case 30:
				return "POWER_STATION";
			case 31:
				return "CONTROL_TOWER";
			case 32:
				return "BOLT_CUTTERS";
			case 33:
				return "GRAPPLING_EQUIPMENT";
			case 34:
				return "GUARD_UNIFORM";
			case 35:
				return "GUARD_TRUCK";
			case 24:
				return "MAIN_GATE";
			case 25:
				return "NORTH_WALL";
			case 26:
				return "NORTH_SIDE_GATE";
			case 27:
				return "SOUTH_WALL";
			case 28:
				return "SOUTH_SIDE_GATE";
			case 29:
				return "DRAINAGE_TUNNEL";
			case 36:
				return "";
			case 37:
				return "";
			case 38:
				return "";
			case 39:
				return "";
			case 40:
				return "SUBMARINE";
			case 41:
				return "BOMBER";
			case 42:
				return "PLANE";
			case 43:
				return "HELICOPTER";
			case 44:
				return "PATROL_BOAT";
			case 45:
				return "SMUGGLER_BOAT";
			case 46:
				return "BOMBER";
			case 47:
				return "PILOT";
			case 48:
				return "HELICOPTER";
			case 49:
				return "PILOT";
			case 50:
				return "DEMOLITION_CHARGES";
			case 53:
				return "ACETYLENE_TORCH";
			case 51:
				return func_388(false, "SAFE_CODE", "PLASMA_CUTTER");
			case 52:
				return "FINGERPRINT";
			case 54:
				return "SHOTGUN";
			case 55:
				return "BULLPUP_RIFLE";
			case 56:
				return "SNIPER";
			case 57:
				return "SMG";
			case 58:
				return "ASSAULT_RIFLE";
			case 59:
				return "SUPPRESSORS";
			case 60:
				return "WEAPONS";
			case 61:
				return "ARMOR";
			case 62:
				return "BACKUP";
			case 63:
				return "";
			case 64:
				return "";
			case 65:
				return "";
			case 66:
				return "";
			case 67:
				return "";
			case 68:
				return "";
			case 69:
				return "";
			case 70:
				return "";
			case 71:
				return "SUBMARINE";
			case 72:
				return "BOMBER";
			case 73:
				return "PLANE";
			case 74:
				return "HELICOPTER";
			case 75:
				return "PATROL_BOAT";
			case 76:
				return "SMUGGLER_BOAT";
			case 77:
				return "AIRSTRIP";
			case 78:
				return "PARACHUTING";
			case 79:
				return "WEST_BEACH";
			case 80:
				return "MAIN_DOCK";
			case 81:
				return "NORTH_DOCK";
			case 82:
				return "NORTH_DROP_ZONE";
			case 83:
				return "SOUTH_DROP_ZONE";
			case 84:
				return "DRAINAGE_TUNNEL";
			case 85:
				return "MAIN_GATE";
			case 86:
				return "DRAINAGE_TUNNEL";
			case 87:
				return "AIRSTRIP";
			case 88:
				return "MAIN_DOCK";
			case 89:
				return "NORTH_DOCK";
			case 90:
				return "SUBMARINE";
			case 91:
				return "";
			case 92:
				return "";
			case 93:
				return "SHOTGUN";
			case 94:
				return "BULLPUP_RIFLE";
			case 95:
				return "SNIPER";
			case 96:
				return "SMG";
			case 97:
				return "ASSAULT_RIFLE";
			case 98:
				return "SUPPRESSORS";
			case 99:
				return "AIRSTRIKE_SUPPORT";
			case 100:
				return "HEAVY_WEAPON_SUPPORT";
			case 101:
				return "SNIPER_SUPPORT";
			case 102:
				return "HELICOPTER_SUPPORT";
			case 103:
				return "SPY_DRONE_SUPPORT";
			case 104:
				return "WEAPON_STASH_SUPPORT";
			case 1000:
				return "PLASMA_CUTTER";
			case 2000:
				return "SAFE_CODE";
			default:
				return "";
			}
		}

		private string func_566(int uParam0, int iParam1)
		{
			switch (iParam1)
			{
			case 1:
				return "H4P_INT0_GAT_D3";
			case 2:
				return "H4P_INT0_LOOT_D";
			case 3:
				return "H4P_INT0_ENTR_D";
			case 4:
				return "H4P_INT0_EXIT_D";
			case 5:
				return "H4P_INT0_COMP_D";
			case 6:
				return "H4P_INT0_ITEM_D";
			case 7:
				return "H4P_INT2_CASH_D";
			case 8:
				return "H4P_INT2_WEED_D";
			case 9:
				return "H4P_INT2_COKE_D";
			case 10:
				return "H4P_INT2_GOLD_D";
			case 11:
				return "H4P_INT2_PAIN_D";
			case 12:
				return "H4P_INT3_AIRS_D";
			case 13:
				return "H4P_INT3_PARA_D";
			case 14:
				return "H4P_INT3_BEAC_D";
			case 15:
				return "H4P_INT3_MDCK_D";
			case 16:
				return "H4P_INT3_NDCK_D";
			case 17:
				return "H4P_INT3_NDRP_D";
			case 18:
				return "H4P_INT3_SDRP_D";
			case 19:
				return "H4P_INT3_DTUN_D";
			case 20:
				return "H4P_INT4_AIRS_D";
			case 21:
				return "H4P_INT4_MDCK_D";
			case 22:
				return "H4P_INT4_NDCK_D";
			case 23:
				return "H4P_INT4_SUBM_D";
			case 30:
				return "H4P_INT6_PWRS_D";
			case 31:
				return "H4P_INT6_CTOW_D";
			case 32:
				return "H4P_INT6_BCUT_D";
			case 33:
				return "H4P_INT6_GRAP_D";
			case 34:
				return "H4P_INT6_UNIF_D";
			case 35:
				return "H4P_INT6_TROJ_D";
			case 24:
				return "H4P_INT5_MGAT_D";
			case 25:
				return "H4P_INT5_NWAL_D";
			case 26:
				return "H4P_INT5_NSGT_D";
			case 27:
				return "H4P_INT5_SWAL_D";
			case 28:
				return "H4P_INT5_SSGT_D";
			case 29:
				return "H4P_INT5_DTUN_D";
			case 36:
				return "H4P_PRP0_APVH_D";
			case 37:
				return "H4P_PRP0_EQUI_D";
			case 38:
				return "H4P_PRP0_WEPL_D";
			case 39:
				return "H4P_PRP0_DISR_D";
			case 40:
				return "H4P_PREP_MAND";
			case 41:
				return "H4P_PREP_OPT";
			case 42:
				return "H4P_PREP_OPT";
			case 43:
				return "H4P_PREP_OPT";
			case 44:
				return "H4P_PREP_OPT";
			case 45:
				return "H4P_PREP_OPT";
			case 46:
				return "H4P_PREP_OPT";
			case 47:
				return "H4P_PREP_OPT";
			case 48:
				return "H4P_PREP_OPT";
			case 49:
				return "H4P_PREP_OPT";
			case 50:
				return "H4P_PREP_OPT";
			case 53:
				return "H4P_PREP_OPT";
			case 51:
				return "H4P_PREP_MAND";
			case 52:
				return "H4P_PREP_MAND";
			case 54:
				return "H4P_PREP_MAND";
			case 55:
				return "H4P_PREP_MAND";
			case 56:
				return "H4P_PREP_MAND";
			case 57:
				return "H4P_PREP_MAND";
			case 58:
				return "H4P_PREP_MAND";
			case 59:
				return "H4P_PRP3_SUPP_D";
			case 60:
				return "H4P_PREP_OPT";
			case 61:
				return "H4P_PREP_OPT";
			case 62:
				return "H4P_PREP_OPT";
			case 63:
				return func_388(false, "H4P_FIN0_APRV_2", "H4P_FIN0_APRV_D");
			case 64:
				return func_388(false, "H4P_FIN0_INFI_2", "H4P_FIN0_INFI_D");
			case 65:
				return func_388(false, "H4P_FIN0_CENT_2", "H4P_FIN0_CENT_D");
			case 66:
				return func_388(false, "H4P_FIN0_ESCA_2", "H4P_FIN0_ESCA_D");
			case 67:
				return func_388(false, "H4P_FIN0_TIMD_2", "H4P_FIN0_TIMD_D");
			case 68:
				return func_388(false, "H4P_FIN0_WEAP_2", "H4P_FIN0_WEAP_D");
			case 69:
				return "H4P_FIN0_SUPP_D";
			case 70:
				return "H4P_FIN0_CREW_D";
			case 71:
				return func_388(false, "H4P_FIN1_SUBM_D", "H4P_FIN1_APRV_D");
			case 72:
				return func_388(false, "H4P_FIN1_BOMB_D", "H4P_FIN1_APRV_D");
			case 73:
				return func_388(false, "H4P_FIN1_SMPL_D", "H4P_FIN1_APRV_D");
			case 74:
				return func_388(false, "H4P_FIN1_SHEL_D", "H4P_FIN1_APRV_D");
			case 75:
				return func_388(false, "H4P_FIN1_PBOA_D", "H4P_FIN1_APRV_D");
			case 76:
				return func_388(false, "H4P_FIN1_SBOA_D", "H4P_FIN1_APRV_D");
			case 77:
				return func_388(false, "H4P_FIN2_AIRS_D", "H4P_FIN2_INFI_D");
			case 78:
				return func_388(false, "H4P_FIN2_PARA_D", "H4P_FIN2_INFI_D");
			case 79:
				return func_388(false, "H4P_FIN2_WBEA_D", "H4P_FIN2_INFI_D");
			case 80:
				return func_388(false, "H4P_FIN2_MDOC_D", "H4P_FIN2_INFI_D");
			case 81:
				return func_388(false, "H4P_FIN2_NDOC_D", "H4P_FIN2_INFI_D");
			case 82:
				return func_388(false, "H4P_FIN2_NDRP_D", "H4P_FIN2_INFI_D");
			case 83:
				return func_388(false, "H4P_FIN2_SDRP_D", "H4P_FIN2_INFI_D");
			case 84:
				return func_388(false, "H4P_FIN2_DTUN_D", "H4P_FIN2_INFI_D");
			case 85:
				return func_388(false, "H4P_FIN3_MGAT_D", "H4P_FIN3_CENT_D");
			case 86:
				return func_388(false, "H4P_FIN3_DTUN_D", "H4P_FIN3_CENT_D");
			case 87:
				return func_388(false, "H4P_FIN4_AIRS_D", "H4P_FIN4_ESCA_D");
			case 88:
				return func_388(false, "H4P_FIN4_MDOC_D", "H4P_FIN4_ESCA_D");
			case 89:
				return func_388(false, "H4P_FIN4_NDOC_D", "H4P_FIN4_ESCA_D");
			case 90:
				return func_388(false, "H4P_FIN4_SUBM_D", "H4P_FIN4_ESCA_D");
			case 91:
				return func_388(false, "H4P_FIN5_TDAY_D", "H4P_FIN5_TIMD_D");
			case 92:
				return func_388(false, "H4P_FIN5_TNGT_D", "H4P_FIN5_TIMD_D");
			case 93:
				return "H4P_FIN6_SHOT_D";
			case 94:
				return "H4P_FIN6_RIFL_D";
			case 95:
				return "H4P_FIN6_SNIP_D";
			case 96:
				return "H4P_FIN6_MKSM_D";
			case 97:
				return "H4P_FIN6_MKAR_D";
			case 98:
				return "H4P_FIN6_SUPP_D";
			case 99:
				return "H4P_PRP3_UKNW_D";
			case 100:
				return "H4P_FIN7_WDRP_D";
			case 101:
				return "H4P_FIN7_SNIP_D";
			case 102:
				return "H4P_FIN7_HELI_D";
			case 103:
				return "H4P_PRP3_UKNW_D";
			case 104:
				return "H4P_FIN7_WEAP_D";
			case 1000:
				return "H4P_PREP_MAND";
			case 2000:
				return "H4P_PREP_MAND";
			default:
				return "";
			}
		}

		private string func_568(int uParam0, int iParam1)
		{
			switch (iParam1)
			{
			case 1:
				return "H4P_INT0_GATH_T";
			case 2:
				return "H4P_INT0_LOOT_T";
			case 3:
				return "H4P_INT0_ENTR_T";
			case 4:
				return "H4P_INT0_EXIT_T";
			case 5:
				return "H4P_INT0_COMP_T";
			case 6:
				return "H4P_INT0_ITEM_T";
			case 7:
				return "H4P_INT2_CASH_T";
			case 8:
				return "H4P_INT2_WEED_T";
			case 9:
				return "H4P_INT2_COKE_T";
			case 10:
				return "H4P_INT2_GOLD_T";
			case 11:
				return "H4P_INT2_PAIN_T";
			case 12:
				return "H4P_INT3_AIRS_T";
			case 13:
				return "H4P_INT3_PARA_T";
			case 14:
				return "H4P_INT3_BEAC_T";
			case 15:
				return "H4P_INT3_MDCK_T";
			case 16:
				return "H4P_INT3_NDCK_T";
			case 17:
				return "H4P_INT3_NDRP_T";
			case 18:
				return "H4P_INT3_SDRP_T";
			case 19:
				return "H4P_INT3_DTUN_T";
			case 20:
				return "H4P_INT4_AIRS_T";
			case 21:
				return "H4P_INT4_MDCK_T";
			case 22:
				return "H4P_INT4_NDCK_T";
			case 23:
				return "H4P_INT4_SUBM_T";
			case 30:
				return "H4P_INT6_PWRS_T";
			case 31:
				return "H4P_INT6_CTOW_T";
			case 32:
				return "H4P_INT6_BCUT_T";
			case 33:
				return "H4P_INT6_GRAP_T";
			case 34:
				return "H4P_INT6_UNIF_T";
			case 35:
				return "H4P_INT6_TROJ_T";
			case 24:
				return "H4P_INT5_MGAT_T";
			case 25:
				return "H4P_INT5_NWAL_T";
			case 26:
				return "H4P_INT5_NSGT_T";
			case 27:
				return "H4P_INT5_SWAL_T";
			case 28:
				return "H4P_INT5_SSGT_T";
			case 29:
				return "H4P_INT5_DTUN_T";
			case 36:
				return "H4P_PRP0_APVH_T";
			case 37:
				return "H4P_PRP0_EQUI_T";
			case 38:
				return "H4P_PRP0_WEPL_T";
			case 39:
				return "H4P_PRP0_DISR_T";
			case 40:
				return "H4P_PRP1_SUBM_T";
			case 41:
				return "H4P_PRP1_SBOM_T";
			case 42:
				return "H4P_PRP1_SPLA_T";
			case 43:
				return "H4P_PRP1_SHEL_T";
			case 44:
				return "H4P_PRP1_PBOA_T";
			case 45:
				return "H4P_PRP1_SBOA_T";
			case 46:
				return "H4P_PRP1_BOM1_T";
			case 47:
				return "H4P_PRP1_BOM2_T";
			case 48:
				return "H4P_PRP1_HEL1_T";
			case 49:
				return "H4P_PRP1_HEL2_T";
			case 50:
				return "H4P_PRP2_DEMC_T";
			case 53:
				return "H4P_PRP2_ATOR_T";
			case 51:
				return func_388(false, "H4P_PRP2_TAR1_T", "H4P_PRP2_TAR2_T");
			case 52:
				return "H4P_PRP2_FING_T";
			case 54:
				return "H4P_PRP3_SHOT_T";
			case 55:
				return "H4P_PRP3_RIFL_T";
			case 56:
				return "H4P_PRP3_SNIP_T";
			case 57:
				return "H4P_PRP3_M2SM_T";
			case 58:
				return "H4P_PRP3_M2RI_T";
			case 59:
				return "H4P_PRP3_SUPP_T";
			case 60:
				return "H4P_PRP4_WEAP_T";
			case 61:
				return "H4P_PRP4_ARMR_T";
			case 62:
				return "H4P_PRP4_BRES_T";
			case 63:
				return "H4P_FIN0_APRV_T";
			case 64:
				return "H4P_FIN0_INFI_T";
			case 65:
				return "H4P_FIN0_CENT_T";
			case 66:
				return "H4P_FIN0_ESCA_T";
			case 67:
				return "H4P_FIN0_TIMD_T";
			case 68:
				return "H4P_FIN0_WEAP_T";
			case 69:
				return "H4P_FIN0_SUPP_T";
			case 70:
				return "H4P_FIN0_CREW_T";
			case 71:
				return "H4P_FIN1_SUBM_T";
			case 72:
				return "H4P_FIN1_BOMB_T";
			case 73:
				return "H4P_FIN1_SMPL_T";
			case 74:
				return "H4P_FIN1_SHEL_T";
			case 75:
				return "H4P_FIN1_PBOA_T";
			case 76:
				return "H4P_FIN1_SBOA_T";
			case 77:
				return "H4P_FIN2_AIRS_T";
			case 78:
				return "H4P_FIN2_PARA_T";
			case 79:
				return "H4P_FIN2_WBEA_T";
			case 80:
				return "H4P_FIN2_MDOC_T";
			case 81:
				return "H4P_FIN2_NDOC_T";
			case 82:
				return "H4P_FIN2_NDRP_T";
			case 83:
				return "H4P_FIN2_SDRP_T";
			case 84:
				return "H4P_FIN2_DTUN_T";
			case 85:
				return "H4P_FIN3_MGAT_T";
			case 86:
				return "H4P_FIN3_DTUN_T";
			case 87:
				return "H4P_FIN4_AIRS_T";
			case 88:
				return "H4P_FIN4_MDOC_T";
			case 89:
				return "H4P_FIN4_NDOC_T";
			case 90:
				return "H4P_FIN4_SUBM_T";
			case 91:
				return "H4P_FIN5_TDAY_T";
			case 92:
				return "H4P_FIN5_TNGT_T";
			case 93:
				return "H4P_FIN6_SHOT_T";
			case 94:
				return "H4P_FIN6_RIFL_T";
			case 95:
				return "H4P_FIN6_SNIP_T";
			case 96:
				return "H4P_FIN6_MKSM_T";
			case 97:
				return "H4P_FIN6_MKAR_T";
			case 98:
				return "H4P_FIN6_SUPP_T";
			case 99:
				return "H4P_FIN7_AIRS_T";
			case 100:
				return "H4P_FIN7_WDRP_T";
			case 101:
				return "H4P_FIN7_SNIP_T";
			case 102:
				return "H4P_FIN7_HELI_T";
			case 103:
				return "H4P_FIN7_SPYD_T";
			case 104:
				return "H4P_FIN7_WEAP_T";
			case 1000:
				return "H4P_PRP2_TAR1_T";
			case 2000:
				return "H4P_PRP2_TAR2_T";
			default:
				return "";
			}
		}

		private int func_569(int uParam0, int uParam1)
		{
			return 0;
		}

		private void func_570(int iParam0, int iParam1)
		{
			Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, iParam0, "REMOVE_FINALE_ITEM");
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, iParam1);
			Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
		}

		private void func_579(int iParam0, int iParam1)
		{
			Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, iParam0, "SET_INPUT_EVENT", (float)iParam1, -1082130432, -1082130432, -1082130432, -1082130432);
		}

		private void func_559(string sParam0)
		{
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_PLAYER_NAME_STRING, sParam0);
		}

		private void func_560(int uParam0, List<dynamic> uParam1, int iParam2, int iParam3, bool Toggle)
		{
			if (uParam1.Count == 0)
			{
				for (int i = 0; i < 8; i++)
				{
					uParam1.Add(0);
				}
			}
			uParam1[0] = iParam2;
			uParam1[1] = iParam3;
			uParam1[2] = 0;
			uParam1[3] = func_568(uParam0, iParam3);
			uParam1[4] = func_566(uParam0, iParam3);
			uParam1[5] = func_563(uParam0, iParam3);
			uParam1[6] = 0;
			uParam1[7] = func_561(iParam3);
			func_558(GetSubTableScaleformHandle(), uParam1, false, Toggle);
		}

		private string func_564(int uParam0)
		{
			switch (uParam0)
			{
			case 0:
				return "GATHER_INTEL_TEQUILA";
			case 1:
				return "GATHER_INTEL_NECKLACE";
			case 2:
				return "GATHER_INTEL_BONDS";
			case 3:
				return "GATHER_INTEL_DIAMOND";
			case 4:
				return "GATHER_INTEL_FILES";
			case 5:
				return "GATHER_INTEL_STATUE";
			default:
				return "GATHER_INTEL";
			}
		}

		private void func_593(int iParam0, bool bParam1, string sParam2)
		{
			Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, iParam0, "SET_LAUNCH_BUTTON");
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_BOOL, bParam1);
			func_413(sParam2);
			Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
		}

		private string func_594(int uParam0)
		{
			switch (uParam0)
			{
			case 0:
				return "H4_FINALE_CONT";
			case 1:
				return "H4_FINALE_RDY";
			default:
				return "H4_FINALE_NRDY";
			}
		}

		private void func_597(int iParam0, string sParam1, string sParam2, string sParam3, string sParam4)
		{
			Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, iParam0, "SHOW_OVERLAY");
			func_413(sParam1);
			func_413(sParam2);
			func_413(sParam3);
			func_413(sParam4);
			Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
		}

		private string func_598()
		{
			return "H4P_OVERLAY_CAN";
		}

		private string func_599()
		{
			return "H4P_OVERLAY_CON";
		}

		private string func_600(int iParam0)
		{
			switch (iParam0)
			{
			case 99:
				return "H4P_OVR_SC1_M";
			case 100:
				return "H4P_OVR_SC2_M";
			case 101:
				return "H4P_OVR_SC3_M";
			case 102:
				return "H4P_OVR_SC4_M";
			case 103:
				return "H4P_OVR_SC5_M";
			case 104:
				return "H4P_OVR_SC6_M";
			default:
				return "";
			}
		}

		private string func_601(int iParam0)
		{
			switch (iParam0)
			{
			case 99:
				return "H4P_OVR_SC1_T";
			case 100:
				return "H4P_OVR_SC2_T";
			case 101:
				return "H4P_OVR_SC3_T";
			case 102:
				return "H4P_OVR_SC4_T";
			case 103:
				return "H4P_OVR_SC5_T";
			case 104:
				return "H4P_OVR_SC6_T";
			default:
				return "";
			}
		}

		private string func_604()
		{
			return "H4P_OVERLAY_ACC";
		}

		private string func_606(int iParam0)
		{
			switch (iParam0)
			{
			case 54:
				return "H4P_OVR_WL1_M";
			case 55:
				return "H4P_OVR_WL2_M";
			case 56:
				return "H4P_OVR_WL3_M";
			case 57:
				return "H4P_OVR_WL4_M";
			case 58:
				return "H4P_OVR_WL5_M";
			default:
				return "";
			}
		}

		private string func_607(int iParam0)
		{
			switch (iParam0)
			{
			case 54:
				return "H4P_OVR_WL1_T";
			case 55:
				return "H4P_OVR_WL2_T";
			case 56:
				return "H4P_OVR_WL3_T";
			case 57:
				return "H4P_OVR_WL4_T";
			case 58:
				return "H4P_OVR_WL5_T";
			default:
				return "";
			}
		}

		private void func_621(int iParam0, int iParam1)
		{
			Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, iParam0, "REMOVE_FINALE_ITEM");
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, iParam1);
			Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
		}

		private void func_633(int iParam0, int iParam1, int iParam2)
		{
			Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, iParam0, "SET_SETUP_ITEM_NUM_COMPLETE");
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, iParam1);
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, iParam2);
			Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
		}

		private void func_804(int iParam0, int iParam1)
		{
			Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, iParam0, "SET_CURRENT_SELECTION");
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, iParam1);
			Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
		}

		private void func_809(int iParam0)
		{
			Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD, iParam0, "HIDE_TIP_TEXT");
		}

		private void func_811(int iParam0, string sParam1)
		{
			Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_STRING, iParam0, "SHOW_TIP_TEXT", sParam1, 0, 0, 0, 0);
		}

		private void func_814(int iParam0, bool bParam1)
		{
			Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, iParam0, "SET_CREW_CUTS_VISIBLE");
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_BOOL, bParam1);
			Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
		}

		private void func_817(int iParam0, int iParam1, int iParam2, int iParam3, int iParam4, bool bParam5)
		{
			Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, iParam0, "SET_CREW_MEMBER");
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, iParam1);
			func_559(Game.Player.Name);
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, iParam3);
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, iParam4);
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_BOOL, bParam5);
			Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
		}

		private void func_838(int iParam0, int iParam1)
		{
			func_842(iParam0);
			Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, iParam0, "SHOW_SCREEN");
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, iParam1);
			Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
		}

		private void func_840(int iParam0, int iParam1)
		{
			Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, iParam0, "SET_INITIAL_TAB_VIEW");
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, iParam1);
			Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
		}

		private void func_842(int iParam0)
		{
			Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, GetSubTableScaleformHandle(), "SET_TABS");
			func_413(func_844(0));
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_BOOL, false);
			func_413(func_844(1));
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_BOOL, !CayoPericoHeist.PlayedIntelSetup);
			func_413(func_844(2));
			CayoPericoHeist.GotApproachVehicle = CayoPericoHeist.Retrieved_Longfin || CayoPericoHeist.Retrieved_PatrolBoat || (CayoPericoHeist.Retrieved_Pilot && CayoPericoHeist.Retrieved_AnnihilatorStealth) || CayoPericoHeist.Retrieved_Velum || (CayoPericoHeist.Retrieved_Pilot && CayoPericoHeist.Retrieved_Alkonost) || CayoPericoHeist.Retrieved_SonarJammer;
			CayoPericoHeist.GotEquipmentLoadout = (CayoPericoHeist.Retrieved_CuttingTorch || CayoPericoHeist.Retrieved_C4) && CayoPericoHeist.Retrieved_FingerprintCloner && (CayoPericoHeist.Retrieved_SafeCodes || CayoPericoHeist.Retrieved_PlasmaCutter);
			CayoPericoHeist.GotWeaponLoadout = CayoPericoHeist.Retrieved_Weapons;
			if (CayoPericoHeist.GotWeaponLoadout && CayoPericoHeist.GotApproachVehicle && CayoPericoHeist.GotEquipmentLoadout)
			{
				CayoPericoHeist.FinaleLocked = false;
			}
			else
			{
				CayoPericoHeist.FinaleLocked = true;
			}
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_BOOL, CayoPericoHeist.FinaleLocked);
			Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
		}

		private string func_844(int iParam0)
		{
			switch (iParam0)
			{
			case 0:
				return "H4P_INTEL_TAB";
			case 1:
				return "H4P_PREP_TAB";
			case 2:
				return "H4P_FINALE_TAB";
			default:
				return "";
			}
		}

		private void func_845(int iParam0, bool bParam1)
		{
			Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, iParam0, "SET_IS_HARD_MODE");
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_BOOL, bParam1);
			Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
		}

		private void func_848(int iParam0, string sParam1)
		{
			Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, iParam0, "SET_FINALE_LIST_TITLE");
			func_413(sParam1);
			Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
		}

		private void func_850(int iParam0, int iParam1, int iParam2, int iParam3, int iParam4, int iParam5, int iParam6, int iParam7, int iParam8)
		{
			Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, iParam0, "SET_FINALE_MENU_IDS");
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, iParam1);
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, iParam2);
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, iParam3);
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, iParam4);
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, iParam5);
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, iParam6);
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, iParam7);
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, iParam8);
			Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
		}

		private void func_851(int iParam0, int iParam1, int iParam2)
		{
			Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, iParam0, "SET_FINALE_COST");
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, iParam1);
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, iParam2);
			Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
		}

		private void func_852(int iParam0, bool bParam1, int iParam2, int iParam3, string sParam4, string sParam5)
		{
			Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, iParam0, "SET_PAYOUTS");
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_BOOL, bParam1);
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, iParam2);
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, iParam3);
			func_413(sParam4);
			func_413(sParam5);
			Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
		}

		private string func_854(int uParam0)
		{
			switch (uParam0)
			{
			case 0:
				return "H4_LOOT_TEQ";
			case 1:
				return "H4_LOOT_NKLC";
			case 2:
				return "H4_LOOT_BONDS";
			case 3:
				return "H4_LOOT_DIAM";
			case 4:
				return "H4_LOOT_FILES";
			case 5:
				return "H4_LOOT_STAT";
			default:
				return "";
			}
		}

		private Vector3 func_868(int iParam0)
		{
			switch (iParam0)
			{
			case 1:
				return new Vector3(4527.591f, -4526.633f, 3.211f);
			case 2:
				return new Vector3(5152.974f, -4619.487f, 1.752f);
			case 3:
				return new Vector3(5155.974f, -5132.699f, 1.312f);
			case 4:
				return new Vector3(4900.456f, -5210.057f, 1.512f);
			case 5:
				return new Vector3(4970.79f, -5695.73f, 18.888f);
			default:
				return new Vector3(0f, 0f, 0f);
			}
		}

		private Vector3 func_870(int iParam0)
		{
			switch (iParam0)
			{
			case 0:
				return new Vector3(4982.59f, -5711.01f, 18.86f);
			case 1:
				return new Vector3(5031.86f, -5685.84f, 23.87f);
			case 2:
				return new Vector3(5085.03f, -5732.67f, 14.8f);
			case 3:
				return new Vector3(4992.24f, -5807.17f, 24.87f);
			case 4:
				return new Vector3(4960.13f, -5785.66f, 19.96f);
			case 5:
				return new Vector3(5045.17f, -5817.24f, -12.7f);
			default:
				return new Vector3(0f, 0f, 0f);
			}
		}

		private int func_871(int iParam0, bool bParam1)
		{
			if (bParam1)
			{
				switch (iParam0)
				{
				case 0:
					return 85;
				case 5:
					return 86;
				}
			}
			else
			{
				switch (iParam0)
				{
				case 0:
					return 24;
				case 1:
					return 25;
				case 2:
					return 26;
				case 3:
					return 27;
				case 4:
					return 28;
				case 5:
					return 29;
				}
			}
			return -1;
		}

		private Vector3 func_872(int iParam0)
		{
			switch (iParam0)
			{
			case 0:
				return new Vector3(4052.4f, -4655.79f, 3.18f);
			case 1:
				return new Vector3(4799.21f, -5157.77f, -4.34f);
			case 2:
				return new Vector3(5176.88f, -4752.2f, -4.11f);
			case 3:
				return new Vector3(4533.25f, -4278.35f, 0f);
			default:
				return new Vector3(0f, 0f, 0f);
			}
		}

		private int func_873(int iParam0, bool bParam1)
		{
			if (bParam1)
			{
				switch (iParam0)
				{
				case 0:
					return 87;
				case 1:
					return 88;
				case 2:
					return 89;
				case 3:
					return 90;
				}
			}
			else
			{
				switch (iParam0)
				{
				case 0:
					return 20;
				case 1:
					return 21;
				case 2:
					return 22;
				case 3:
					return 23;
				}
			}
			return -1;
		}

		private Vector3 func_874(int iParam0)
		{
			switch (iParam0)
			{
			case 0:
				return new Vector3(4052.4f, -4655.79f, 3.18f);
			case 3:
				return new Vector3(4799.21f, -5157.77f, -4.34f);
			case 4:
				return new Vector3(5176.88f, -4752.2f, -4.11f);
			case 2:
				return new Vector3(4891.13f, -4925.02f, 9.31f);
			case 5:
				return new Vector3(4820.73f, -4302.71f, 4.24f);
			case 6:
				return new Vector3(5478.63f, -5847.74f, 19.54f);
			case 7:
				return new Vector3(5045.17f, -5817.24f, -12.7f);
			default:
				return new Vector3(0f, 0f, 0f);
			}
		}

		private int func_875(int iParam0)
		{
			if (iParam0 == 1)
			{
				return 9;
			}
			return 0;
		}

		private int func_876(int iParam0, bool bParam1)
		{
			if (bParam1)
			{
				switch (iParam0)
				{
				case 0:
					return 77;
				case 1:
					return 78;
				case 2:
					return 79;
				case 3:
					return 80;
				case 4:
					return 81;
				case 5:
					return 82;
				case 6:
					return 83;
				case 7:
					return 84;
				}
			}
			else
			{
				switch (iParam0)
				{
				case 0:
					return 12;
				case 1:
					return 13;
				case 2:
					return 14;
				case 3:
					return 15;
				case 4:
					return 16;
				case 5:
					return 17;
				case 6:
					return 18;
				case 7:
					return 19;
				}
			}
			return -1;
		}

		private Vector3 func_877(int iParam0)
		{
			switch (iParam0)
			{
			case 0:
				return new Vector3(4534.709f, -4543.447f, 4.53f);
			case 1:
				return new Vector3(4523.664f, -4511.79f, 4.184f);
			case 2:
				return new Vector3(4076.849f, -4667.305f, 4.163f);
			case 3:
				return new Vector3(4505.6f, -4653.089f, 10.456f);
			case 4:
				return new Vector3(4805.025f, -4315.458f, 6.514f);
			case 5:
				return new Vector3(5180.197f, -4670.979f, 6.231f);
			case 6:
				return new Vector3(5098.02f, -4621.306f, 2.594f);
			case 7:
				return new Vector3(5070.327f, -4638.299f, 2.902f);
			case 8:
				return new Vector3(4955.743f, -5181.93f, 4.512f);
			case 9:
				return new Vector3(5216.539f, -5126.448f, 5.984f);
			case 10:
				return new Vector3(4879.029f, -5112.622f, 1.995f);
			case 11:
				return new Vector3(4901.985f, -5348.011f, 9.409f);
			case 12:
				return new Vector3(5362.94f, -5437.508f, 48.491f);
			case 13:
				return new Vector3(5466.287f, -5232.051f, 27.065f);
			case 14:
				return new Vector3(4754.99f, -5541.311f, 18.056f);
			case 15:
				return new Vector3(5326.267f, -5266.071f, 32.237f);
			default:
				return new Vector3(0f, 0f, 0f);
			}
		}

		private Vector3 func_883(int iParam0)
		{
			switch (iParam0)
			{
			case 0:
				return new Vector3(5163.384f, -4995.56f, 11.682f);
			case 1:
				return new Vector3(4765.362f, -4778.338f, 2.781f);
			case 2:
				return new Vector3(4502.198f, -4523.357f, 3.396f);
			case 3:
				return new Vector3(4528.522f, -4536.311f, 6.558f);
			case 4:
				return new Vector3(5098.933f, -4609.03f, 1.369f);
			case 5:
				return new Vector3(5060.167f, -4589.7f, 1.9f);
			case 6:
				return new Vector3(5148.113f, -4616.131f, 1.387f);
			case 7:
				return new Vector3(5170.703f, -4675.337f, 1.439f);
			case 8:
				return new Vector3(5116.664f, -5130.588f, 1.143f);
			case 9:
				return new Vector3(5002.533f, -5125.336f, 1.955f);
			case 10:
				return new Vector3(4949.935f, -5321.81f, 7.085f);
			case 11:
				return new Vector3(5012.355f, -5203.456f, 1.516f);
			case 12:
				return new Vector3(4923.873f, -5273.464f, 4.65f);
			case 13:
				return new Vector3(5263.995f, -5435.548f, 64.881f);
			case 14:
				return new Vector3(4886.995f, -5454.333f, 29.731f);
			case 15:
				return new Vector3(5106.183f, -5524.682f, 53.239f);
			default:
				return new Vector3(0f, 0f, 0f);
			}
		}

		private Vector3 func_889(int iParam0)
		{
			switch (iParam0)
			{
			case 0:
				return new Vector3(4896.678f, -4791.297f, 2.59f);
			case 1:
				return new Vector3(4529.568f, -4703.347f, 3.134f);
			case 2:
				return new Vector3(4493.661f, -4733.618f, 10.01f);
			case 3:
				return new Vector3(3895.499f, -4695.022f, 5.547f);
			case 4:
				return new Vector3(5103.035f, -4681.288f, 7.702f);
			case 5:
				return new Vector3(5130.433f, -4610.441f, 11.724f);
			case 6:
				return new Vector3(5109.232f, -4578.338f, 28.711f);
			case 7:
				return new Vector3(4879.964f, -4487.782f, 9.922f);
			case 8:
				return new Vector3(4901.395f, -5331.422f, 28.64f);
			case 9:
				return new Vector3(4862.909f, -5158.419f, 2.283f);
			case 10:
				return new Vector3(5124.89f, -5097.902f, 2.192f);
			case 11:
				return new Vector3(4867.519f, -4642.129f, 13.571f);
			case 12:
				return new Vector3(5568.438f, -5185.942f, 10.22f);
			case 13:
				return new Vector3(5406.297f, -5170.91f, 31.198f);
			case 14:
				return new Vector3(5265.993f, -5430.593f, 140.566f);
			case 15:
				return new Vector3(5611.286f, -5654.516f, 9.051f);
			default:
				return new Vector3(0f, 0f, 0f);
			}
		}

		private void func_582(int iParam0, int iParam1, int iParam2)
		{
			Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, iParam0, "SET_SETUP_ITEM_NUM_COMPLETE");
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, iParam1);
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, iParam2);
			Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
		}

		private Vector3 func_895(int iParam0)
		{
			switch (iParam0)
			{
			case 0:
				return new Vector3(5081.14f, -5758.794f, 15.981f);
			case 1:
				return new Vector3(5027.032f, -5738.977f, 18.027f);
			case 2:
				return new Vector3(5008.773f, -5783.208f, 17.99f);
			case 3:
				return new Vector3(5014.587f, -5751.069f, 29.006f);
			case 4:
				return new Vector3(5004.83f, -5755.521f, 29.006f);
			case 5:
				return new Vector3(4995.553f, -5748.032f, 15.002f);
			case 6:
				return new Vector3(4997.112f, -5745.52f, 15.001f);
			default:
				return new Vector3(0f, 0f, 0f);
			}
		}

		private Vector3 func_898(int iParam0)
		{
			switch (iParam0)
			{
			case 0:
				return new Vector3(5081.753f, -5754.63f, 15.764f);
			case 1:
				return new Vector3(5082.945f, -5758.447f, 15.765f);
			case 2:
				return new Vector3(5029.346f, -5733.933f, 17.81f);
			case 3:
				return new Vector3(5029.777f, -5737.731f, 17.808f);
			case 4:
				return new Vector3(5012.031f, -5788.325f, 17.773f);
			case 5:
				return new Vector3(5006.233f, -5785.789f, 17.771f);
			case 6:
				return new Vector3(4998.106f, -5752.379f, 14.783f);
			case 7:
				return new Vector3(5003.947f, -5748.77f, 14.787f);
			default:
				return new Vector3(0f, 0f, 0f);
			}
		}

		private void func_902(int iParam0, int iParam1, int iParam2, int iParam3, int iParam4, Vector2 Param5)
		{
			Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, iParam0, "ADD_WORLD_MAP_ICON");
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, iParam1);
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, iParam2);
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, iParam3);
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, iParam4);
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_FLOAT, Param5.X);
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_FLOAT, Param5.Y);
			Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
		}

		private Vector3 func_903(int iParam0)
		{
			switch (iParam0)
			{
			case 0:
				return new Vector3(4447.822f, -4442.135f, 7.175f);
			case 1:
				return new Vector3(4448.821f, -4444.858f, 7.182f);
			case 2:
				return new Vector3(4435.688f, -4446.595f, 4.25f);
			case 3:
				return new Vector3(4438.332f, -4445.811f, 4.267f);
			case 4:
				return new Vector3(4506.064f, -4555.47f, 4.095f);
			case 5:
				return new Vector3(4503.678f, -4556.323f, 4.096f);
			case 6:
				return new Vector3(5064.635f, -4589.759f, 2.801f);
			case 7:
				return new Vector3(5067.557f, -4590.849f, 2.795f);
			case 8:
				return new Vector3(5092.602f, -4680.137f, 2.35f);
			case 9:
				return new Vector3(5093.195f, -4683.354f, 2.35f);
			case 10:
				return new Vector3(5091.02f, -4685.678f, 2.351f);
			case 11:
				return new Vector3(5136.102f, -4613.863f, 2.401f);
			case 12:
				return new Vector3(5131.834f, -4612.648f, 2.404f);
			case 13:
				return new Vector3(5329.472f, -5272.372f, 33.13f);
			case 14:
				return new Vector3(5328.096f, -5270.587f, 33.129f);
			case 15:
				return new Vector3(5196.664f, -5133.933f, 3.284f);
			case 16:
				return new Vector3(5196.121f, -13962f / MathF.E, 3.285f);
			case 17:
				return new Vector3(5000.313f, -5163.344f, 2.697f);
			case 18:
				return new Vector3(5001.3f, -5165.434f, 2.697f);
			case 19:
				return new Vector3(4959.845f, -5107.064f, 2.911f);
			case 20:
				return new Vector3(4962.675f, -5106.771f, 2.913f);
			case 21:
				return new Vector3(4963.856f, -5109.32f, 2.912f);
			case 22:
				return new Vector3(4926.316f, -5244.514f, 2.461f);
			case 23:
				return new Vector3(4924.385f, -5245.882f, 2.461f);
			default:
				return new Vector3(0f, 0f, 0f);
			}
		}

		private int func_904(int iParam0)
		{
			switch (iParam0)
			{
			case 0:
				return 4;
			case 1:
				return 3;
			case 2:
				return 6;
			case 3:
				return 5;
			default:
				return -1;
			}
		}

		private int func_905(int iParam0)
		{
			switch (iParam0)
			{
			case 0:
				return 7;
			case 1:
				return 8;
			case 2:
				return 9;
			case 3:
				return 10;
			default:
				return -1;
			}
		}

		private void func_910(int iParam0, List<dynamic> Param1)
		{
			if (Param1.Count == 0)
			{
				return;
			}
			func_582(iParam0, Param1[0], 0);
			Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, iParam0, "ADD_SETUP_ITEM");
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, Param1[0]);
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, Param1[1]);
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, Param1[2]);
			func_413(Param1[3].ToString());
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, Param1[4]);
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, Param1[5]);
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, Param1[6]);
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_BOOL, Param1[7]);
			func_413(Param1[8].ToString());
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_TEXTURE_NAME_STRING, Param1[9]);
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_BOOL, false);
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, Param1[11]);
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, Param1[12]);
			Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
			string text = "";
			foreach (dynamic item in Param1)
			{
				text = (string)(text + (item.ToString() + " , "));
			}
		}

		private void func_913(int uParam0, List<dynamic> uParam1, int iParam2, int iParam3, int iParam4, int bParam5)
		{
			if (uParam1.Count == 0)
			{
				for (int i = 0; i < 13; i++)
				{
					uParam1.Add(0);
				}
			}
			uParam1[0] = iParam2;
			uParam1[1] = iParam3;
			uParam1[2] = iParam4;
			if (bParam5 == 1)
			{
				uParam1[3] = "CH_LIST_BLANK";
				uParam1[4] = 0;
				uParam1[5] = 0;
				uParam1[6] = 2;
				uParam1[7] = 0;
				uParam1[8] = "CH_LIST_BLANK";
				uParam1[9] = "";
				uParam1[10] = 0;
				uParam1[11] = -1;
				uParam1[12] = -1;
			}
			else
			{
				uParam1[3] = func_568(uParam0, iParam4);
				uParam1[4] = 0;
				uParam1[5] = func_930(uParam0, iParam4);
				uParam1[6] = 0;
				uParam1[7] = 0;
				uParam1[8] = func_566(uParam0, iParam4);
				uParam1[9] = func_568(uParam0, iParam4);
				uParam1[10] = func_561(iParam4);
				uParam1[11] = -1;
				uParam1[12] = -1;
			}
			if (uParam1.Count != 0)
			{
				func_910(uParam0, uParam1);
			}
		}

		private int func_924(bool bParam0)
		{
			if (bParam0)
			{
				return 1;
			}
			return 0;
		}

		private int func_930(int uParam0, int iParam1)
		{
			switch (iParam1)
			{
			case 1:
				return 0;
			case 2:
				return 0;
			case 3:
				return 0;
			case 4:
				return 0;
			case 5:
				return 0;
			case 6:
				return 0;
			case 7:
				return 1;
			case 8:
				return 1;
			case 9:
				return 1;
			case 10:
				return 1;
			case 11:
				return 1;
			case 12:
				return 1;
			case 13:
				return 1;
			case 14:
				return 1;
			case 15:
				return 1;
			case 16:
				return 1;
			case 17:
				return 1;
			case 18:
				return 1;
			case 19:
				return 1;
			case 20:
				return 1;
			case 21:
				return 1;
			case 22:
				return 1;
			case 23:
				return 1;
			case 30:
				return 1;
			case 31:
				return 1;
			case 32:
				return 1;
			case 33:
				return 1;
			case 34:
				return 1;
			case 35:
				return 1;
			case 24:
				return 1;
			case 25:
				return 1;
			case 26:
				return 1;
			case 27:
				return 1;
			case 28:
				return 1;
			case 29:
				return 1;
			case 36:
				return 1;
			case 37:
				return 1;
			case 38:
				return 1;
			case 39:
				return 1;
			case 40:
				return 1;
			case 41:
				return 1;
			case 42:
				return 1;
			case 43:
				return 1;
			case 44:
				return 1;
			case 45:
				return 1;
			case 46:
				return 1;
			case 47:
				return 1;
			case 48:
				return 1;
			case 49:
				return 1;
			case 50:
				return 1;
			case 53:
				return 1;
			case 51:
				return 1;
			case 1000:
				return 1;
			case 2000:
				return 1;
			case 52:
				return 1;
			case 54:
				return 1;
			case 55:
				return 1;
			case 56:
				return 1;
			case 57:
				return 1;
			case 58:
				return 1;
			case 59:
				return 1;
			case 60:
				return 1;
			case 61:
				return 1;
			case 62:
				return 1;
			default:
				return 0;
			}
		}

		private string func_937(bool uParam0)
		{
			if (uParam0)
			{
				return "HIP_PROMPT2";
			}
			return "HIP_PROMPT";
		}

		public bool InttoBool(int i)
		{
			bool result = false;
			if (i == 0)
			{
				result = false;
			}
			if (i == 1)
			{
				result = true;
			}
			return result;
		}

		public int BoolToInt(bool i)
		{
			int result = 0;
			if (!i)
			{
				result = 0;
			}
			if (i)
			{
				result = 1;
			}
			return result;
		}

		public void DisplayHelpTextThisFrame(string helpText, int Shape)
		{
			Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_HELP, "CELL_EMAIL_BCON");
			int num = 99;
			for (int i = 0; i < helpText.Length; i += num)
			{
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, helpText.Substring(i, Math.Min(num, helpText.Length - i)));
			}
			Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_HELP, 0, 0, 1, Shape);
		}

		public void DisplayHelpTextThisFrameNoSound(string helpText, int Shape)
		{
			Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_HELP, "CELL_EMAIL_BCON");
			int num = 99;
			for (int i = 0; i < helpText.Length; i += num)
			{
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, helpText.Substring(i, Math.Min(num, helpText.Length - i)));
			}
			Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_HELP, 0, 0, 0, Shape);
		}

		public string ReturnButtonString(int ID)
		{
			SelectedButtonIndex = "Error Invald Key ID";
			if (ID < 358)
			{
				foreach (ButtonData item in ButtonIndex)
				{
					if (item.ButtonID == ID)
					{
						SelectedButtonIndex = item.DisplayName;
						string selectedButtonIndex = SelectedButtonIndex;
						SelectedButtonIndex = "~" + selectedButtonIndex + "~";
					}
				}
			}
			else
			{
				SelectedButtonIndex = "Error Invald Key ID :" + ID;
			}
			return SelectedButtonIndex;
		}

		public int ReturnButtonID(int ID)
		{
			return ID;
		}

		private void drawText(string text, float x, float y, float scale, int r, int g, int b)
		{
			Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_TEXT, "STRING");
			Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, text);
			Function.Call(Hash.SET_TEXT_COLOUR, r, g, b, 255);
			Function.Call(Hash.SET_TEXT_SCALE, 0f, scale);
			Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_TEXT, x, y, 0.1);
		}

		private void drawTextWithFont(string text, float x, float y, float scale, int r, int g, int b, Font F)
		{
			Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_TEXT, "STRING");
			Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, text);
			Function.Call(Hash.SET_TEXT_COLOUR, r, g, b, 255);
			Function.Call(Hash.SET_TEXT_SCALE, 0f, scale);
			Function.Call(Hash.SET_TEXT_FONT, (int)F);
			Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_TEXT, x, y, 0.1);
		}

		public static void TextNotification(string avatar, string author, string title, string message)
		{
			Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "CONFIRM_BEEP", "HUD_MINI_GAME_SOUNDSET");
			Function.Call(Hash.BEGIN_TEXT_COMMAND_THEFEED_POST, "CELL_EMAIL_BCON");
			int num = 99;
			for (int i = 0; i < message.Length; i += num)
			{
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, message.Substring(i, Math.Min(num, message.Length - i)));
			}
			int num2 = Function.Call<int>(Hash.END_TEXT_COMMAND_THEFEED_POST_MESSAGETEXT, new InputArgument[6] { avatar, avatar, true, 0, title, author });
		}

		public void CreateSetupMenus()
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int subTableScaleformHandle = GetSubTableScaleformHandle();
			List<object> list = new List<object>();
			IntelItems.Add(list);
			func_913(subTableScaleformHandle, list, 0, 0, 1, 0);
			list = new List<object>();
			IntelItems.Add(list);
			func_913(subTableScaleformHandle, list, 0, 0, 2, 0);
			list = new List<object>();
			IntelItems.Add(list);
			func_913(subTableScaleformHandle, list, 0, 0, 3, 0);
			list = new List<object>();
			IntelItems.Add(list);
			func_913(subTableScaleformHandle, list, 0, 0, 4, 0);
			list = new List<object>();
			IntelItems.Add(list);
			func_913(subTableScaleformHandle, list, 0, 0, 5, 0);
			list = new List<object>();
			IntelItems.Add(list);
			func_913(subTableScaleformHandle, list, 0, 0, 6, 0);
			num = 0;
			list = new List<object>();
			IntelItems.Add(list);
			func_913(subTableScaleformHandle, list, 0, 2, 7, 0);
			list = new List<object>();
			IntelItems.Add(list);
			func_913(subTableScaleformHandle, list, 0, 2, 8, 0);
			list = new List<object>();
			IntelItems.Add(list);
			func_913(subTableScaleformHandle, list, 0, 2, 9, 0);
			list = new List<object>();
			IntelItems.Add(list);
			func_913(subTableScaleformHandle, list, 0, 2, 10, 0);
			list = new List<object>();
			IntelItems.Add(list);
			func_913(subTableScaleformHandle, list, 0, 2, 11, 0);
			list = new List<object>();
			IntelItems.Add(list);
			func_913(subTableScaleformHandle, list, 0, 2, 12, 0);
			if (num2 == 0)
			{
				list = new List<object>();
				IntelItems.Add(list);
				func_913(subTableScaleformHandle, list, 0, 2, 7, 1);
			}
			num2 = 0;
			for (num = 0; num < 8; num++)
			{
				num3 = 12 + num;
				num2++;
			}
			list = new List<object>();
			IntelItems.Add(list);
			func_913(subTableScaleformHandle, list, 0, 3, 12, 0);
			list = new List<object>();
			IntelItems.Add(list);
			func_913(subTableScaleformHandle, list, 0, 3, 13, 0);
			list = new List<object>();
			IntelItems.Add(list);
			func_913(subTableScaleformHandle, list, 0, 3, 14, 0);
			list = new List<object>();
			IntelItems.Add(list);
			func_913(subTableScaleformHandle, list, 0, 3, 15, 0);
			list = new List<object>();
			IntelItems.Add(list);
			func_913(subTableScaleformHandle, list, 0, 3, 16, 0);
			list = new List<object>();
			IntelItems.Add(list);
			func_913(subTableScaleformHandle, list, 0, 3, 17, 0);
			list = new List<object>();
			IntelItems.Add(list);
			func_913(subTableScaleformHandle, list, 0, 3, 18, 0);
			list = new List<object>();
			IntelItems.Add(list);
			func_913(subTableScaleformHandle, list, 0, 3, 19, 0);
			num2 = 0;
			for (num = 0; num < 4; num++)
			{
				num3 = 20 + num;
				list = new List<object>();
				IntelItems.Add(list);
				func_913(subTableScaleformHandle, list, 0, 4, num3, 0);
				num2++;
			}
			num2 = 0;
			for (num = 0; num < 6; num++)
			{
				num3 = 24 + num;
				list = new List<object>();
				IntelItems.Add(list);
				func_913(subTableScaleformHandle, list, 0, 5, num3, 0);
				num2++;
			}
			num2 = 0;
			list = new List<object>();
			IntelItems.Add(list);
			func_913(subTableScaleformHandle, list, 0, 6, 30, 0);
			num2++;
			list = new List<object>();
			IntelItems.Add(list);
			func_913(subTableScaleformHandle, list, 0, 6, 31, 0);
			num2++;
			list = new List<object>();
			IntelItems.Add(list);
			func_913(subTableScaleformHandle, list, 0, 6, 32, 0);
			num2++;
			list = new List<object>();
			IntelItems.Add(list);
			func_913(subTableScaleformHandle, list, 0, 6, 33, 0);
			num2++;
			list = new List<object>();
			IntelItems.Add(list);
			func_913(subTableScaleformHandle, list, 0, 6, 34, 0);
			num2++;
			list = new List<object>();
			IntelItems.Add(list);
			func_913(subTableScaleformHandle, list, 0, 6, 35, 0);
			num2++;
			list = new List<object>();
			IntelItems.Add(list);
			func_913(subTableScaleformHandle, list, 0, 6, 32, 1);
			list = new List<object>();
			SetupItems.Add(list);
			func_913(subTableScaleformHandle, list, 1, 36, 40, 0);
			list = new List<object>();
			SetupItems.Add(list);
			func_913(subTableScaleformHandle, list, 1, 36, 41, 0);
			list = new List<object>();
			SetupItems.Add(list);
			func_913(subTableScaleformHandle, list, 1, 36, 42, 0);
			list = new List<object>();
			SetupItems.Add(list);
			func_913(subTableScaleformHandle, list, 1, 36, 43, 0);
			list = new List<object>();
			SetupItems.Add(list);
			func_913(subTableScaleformHandle, list, 1, 36, 44, 0);
			list = new List<object>();
			SetupItems.Add(list);
			func_913(subTableScaleformHandle, list, 1, 36, 45, 0);
			list = new List<object>();
			SetupItems.Add(list);
			func_913(subTableScaleformHandle, list, 1, 41, 46, 0);
			list = new List<object>();
			SetupItems.Add(list);
			func_913(subTableScaleformHandle, list, 1, 41, 47, 0);
			list = new List<object>();
			SetupItems.Add(list);
			func_913(subTableScaleformHandle, list, 1, 43, 48, 0);
			list = new List<object>();
			SetupItems.Add(list);
			func_913(subTableScaleformHandle, list, 1, 43, 49, 0);
			list = new List<object>();
			SetupItems.Add(list);
			func_913(subTableScaleformHandle, list, 1, 0, 36, 0);
			list = new List<object>();
			SetupItems.Add(list);
			func_913(subTableScaleformHandle, list, 1, 0, 37, 0);
			list = new List<object>();
			SetupItems.Add(list);
			func_913(subTableScaleformHandle, list, 1, 0, 38, 0);
			list = new List<object>();
			SetupItems.Add(list);
			func_913(subTableScaleformHandle, list, 1, 0, 39, 0);
			list = new List<object>();
			SetupItems.Add(list);
			func_913(subTableScaleformHandle, list, 1, 37, 50, 0);
			list = new List<object>();
			SetupItems.Add(list);
			func_913(subTableScaleformHandle, list, 1, 37, 1000, 0);
			list = new List<object>();
			SetupItems.Add(list);
			func_913(subTableScaleformHandle, list, 1, 37, 2000, 0);
			list = new List<object>();
			SetupItems.Add(list);
			func_913(subTableScaleformHandle, list, 1, 37, 52, 0);
			list = new List<object>();
			SetupItems.Add(list);
			func_913(subTableScaleformHandle, list, 1, 37, 53, 0);
			for (num2 = 0; num2 < 5; num2++)
			{
				num3 = 54 + num2;
				list = new List<object>();
				SetupItems.Add(list);
				func_913(subTableScaleformHandle, list, 1, 38, num3, 0);
				num++;
			}
			list = new List<object>();
			SetupItems.Add(list);
			func_913(subTableScaleformHandle, list, 1, 38, 59, 0);
			list = new List<object>();
			SetupItems.Add(list);
			func_913(subTableScaleformHandle, list, 1, 39, 60, 0);
			list = new List<object>();
			SetupItems.Add(list);
			func_913(subTableScaleformHandle, list, 1, 39, 61, 0);
			list = new List<object>();
			SetupItems.Add(list);
			func_913(subTableScaleformHandle, list, 1, 39, 62, 0);
			for (; num < 24; num++)
			{
				func_902(subTableScaleformHandle, func_905(0), -1, func_904(0), 0, func_903(num));
			}
			for (num2 = 0; num2 < 8; num2++)
			{
				func_902(subTableScaleformHandle, func_905(0), -1, func_904(0), 1, func_898(num2));
			}
			for (num3 = 0; num3 < 7; num3++)
			{
				func_902(subTableScaleformHandle, 11, -1, 7, 1, func_895(num3));
			}
			for (int i = 0; i < 4; i++)
			{
				func_902(subTableScaleformHandle, 33, -1, 2, 0, func_889(i));
			}
			for (int i = 0; i < 4; i++)
			{
				func_902(subTableScaleformHandle, 34, -1, 2, 0, func_883(i));
			}
			for (int i = 0; i < 8; i++)
			{
				func_902(GetSubTableScaleformHandle(), func_876(i, false), func_876(i, true), func_875(i), 0, func_874(i));
			}
			for (int i = 0; i < 4; i++)
			{
				func_902(GetSubTableScaleformHandle(), func_873(i, false), func_873(i, true), 1, 0, func_872(i));
			}
			for (int i = 0; i < 6; i++)
			{
				func_902(GetSubTableScaleformHandle(), func_871(i, false), func_871(i, true), 0, 1, func_870(i));
			}
			num = 0;
			func_902(GetSubTableScaleformHandle(), 31, -1, 2, 0, new Vector3(4367.82f, -4579f, 23.06f));
			func_902(GetSubTableScaleformHandle(), 30, -1, 2, 0, new Vector3(4480.7f, -4588.52f, 4.56f));
			func_902(GetSubTableScaleformHandle(), 5, -1, 8, 0, new Vector3(5020.635f, -5746.534f, 31.93707f));
			func_902(GetSubTableScaleformHandle(), 35, -1, 2, 0, func_868(1));
			OSFScreen++;
			Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, GetSubTableScaleformHandle(), "SHOW_SCREEN");
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, OSFScreen);
			Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
			if (CayoPericoHeist.GotWeaponLoadout && CayoPericoHeist.GotApproachVehicle && CayoPericoHeist.GotEquipmentLoadout)
			{
				CayoPericoHeist.FinaleLocked = false;
			}
			func_842(GetSubTableScaleformHandle());
			list = new List<object>();
			SetupItems.Add(list);
			func_913(subTableScaleformHandle, list, 1, 36, 40, 0);
			list = new List<object>();
			SetupItems.Add(list);
			func_913(subTableScaleformHandle, list, 1, 36, 41, 0);
			list = new List<object>();
			SetupItems.Add(list);
			func_913(subTableScaleformHandle, list, 1, 36, 42, 0);
			list = new List<object>();
			SetupItems.Add(list);
			func_913(subTableScaleformHandle, list, 1, 36, 43, 0);
			list = new List<object>();
			SetupItems.Add(list);
			func_913(subTableScaleformHandle, list, 1, 36, 44, 0);
			list = new List<object>();
			SetupItems.Add(list);
			func_913(subTableScaleformHandle, list, 1, 36, 45, 0);
			list = new List<object>();
			SetupItems.Add(list);
			func_913(subTableScaleformHandle, list, 1, 41, 46, 0);
			list = new List<object>();
			SetupItems.Add(list);
			func_913(subTableScaleformHandle, list, 1, 41, 47, 0);
			list = new List<object>();
			SetupItems.Add(list);
			func_913(subTableScaleformHandle, list, 1, 43, 48, 0);
			list = new List<object>();
			SetupItems.Add(list);
			func_913(subTableScaleformHandle, list, 1, 43, 49, 0);
		}

		private void CreateTableScreen()
		{
			Game.DisableControlThisFrame(Control.Phone);
			switch (OStage)
			{
			case 0:
			{
				IntelItems.Clear();
				SetupItems.Clear();
				FinaleItems.Clear();
				Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "HS4_SUB_INT", false);
				Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "HS4_SUB_INT_BOOT", false);
				Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "HS4_SUB_INT_BOOT_2", false);
				Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "HS4_SUB_INT_EL_RUBIO", false);
				Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "HS4_SUB_INT_EL_RUBIO_1", false);
				Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "HS4_SUB_INT_EL_RUBIO_2", false);
				Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "HS4_SUB_INT_EL_RUBIO_3", false);
				Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "HS4_SUB_INT_EL_RUBIO_4", false);
				Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "HS4_SUB_INT_EL_RUBIO_5", false);
				Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "HS4_SUB_INT_EL_RUBIO_6", false);
				while (!Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, "HS4_SUB_INT_BOOT"))
				{
					drawText("Loading Streemed Dictionaries, ISLAND_HEIST_BOARD ", 0.1f, 0.2f, 0.3f, 255, 255, 255);
					Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "HS4_SUB_INT", false);
					Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "HS4_SUB_INT_BOOT", false);
					Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "HS4_SUB_INT_BOOT_2", false);
					Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "HS4_SUB_INT_EL_RUBIO", false);
					Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "HS4_SUB_INT_EL_RUBIO_1", false);
					Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "HS4_SUB_INT_EL_RUBIO_2", false);
					Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "HS4_SUB_INT_EL_RUBIO_3", false);
					Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "HS4_SUB_INT_EL_RUBIO_4", false);
					Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "HS4_SUB_INT_EL_RUBIO_5", false);
					Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "HS4_SUB_INT_EL_RUBIO_6", false);
					Script.Wait(1);
				}
				if (UseScaleformVarInsteadOfInt && SubTableScale != null)
				{
					SubTableScale.Dispose();
				}
				if (UseScaleformVarInsteadOfInt)
				{
					SubTableScale = new Scaleform("ISLAND_HEIST_BOARD");
					while (!SubTableScale.IsLoaded)
					{
						SubTableScale = new Scaleform("ISLAND_HEIST_BOARD");
						Script.Wait(1);
					}
				}
				func_524(GetSubTableScaleformHandle());
				string text = "submarine_table";
				uint num9 = 771658040u;
				if (Function.Call<bool>(Hash.IS_NAMED_RENDERTARGET_REGISTERED, text))
				{
					Function.Call(Hash.RELEASE_NAMED_RENDERTARGET, text);
				}
				Function.Call(Hash.REGISTER_NAMED_RENDERTARGET, text, false);
				if (!Function.Call<bool>(Hash.IS_NAMED_RENDERTARGET_LINKED, num9))
				{
					Function.Call(Hash.LINK_NAMED_RENDERTARGET, num9);
					OSF1 = Function.Call<int>(Hash.GET_NAMED_RENDERTARGET_RENDER_ID, text);
					OSFScreen = 0;
					if (OSF1 != -1 && OSF1 != 0)
					{
						OStage = 1;
					}
				}
				else
				{
					Function.Call(Hash.LINK_NAMED_RENDERTARGET, num9);
					OSF1 = Function.Call<int>(Hash.GET_NAMED_RENDERTARGET_RENDER_ID, text);
					if (OSF1 != -1 && OSF1 != 0)
					{
						OStage = 1;
					}
				}
				CreateSetupMenus();
				Notification.Show("~o~Adding Table Menus~w~");
				break;
			}
			case 1:
			{
				if (!Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, GetSubTableScaleformHandle()))
				{
					break;
				}
				Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, GetSubTableScaleformHandle(), "SHOW_SCREEN");
				Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, 1);
				Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
				if (CayoPericoHeist.GotWeaponLoadout && CayoPericoHeist.GotApproachVehicle && CayoPericoHeist.GotEquipmentLoadout)
				{
					CayoPericoHeist.FinaleLocked = false;
				}
				func_842(GetSubTableScaleformHandle());
				int num4 = 0;
				int num5 = 0;
				int num6 = 0;
				int subTableScaleformHandle2 = GetSubTableScaleformHandle();
				for (int i = 62; i < 110; i++)
				{
					func_570(GetSubTableScaleformHandle(), i);
				}
				func_850(GetSubTableScaleformHandle(), 67, 63, 64, 66, 68, 69, 70, 65);
				List<object> list = new List<object>();
				FinaleItems.Add(list);
				func_560(subTableScaleformHandle2, list, 0, 63, false);
				list = new List<object>();
				FinaleItems.Add(list);
				func_560(subTableScaleformHandle2, list, 0, 64, true);
				list = new List<object>();
				FinaleItems.Add(list);
				func_560(subTableScaleformHandle2, list, 0, 65, false);
				list = new List<object>();
				FinaleItems.Add(list);
				func_560(subTableScaleformHandle2, list, 0, 66, false);
				list = new List<object>();
				FinaleItems.Add(list);
				func_560(subTableScaleformHandle2, list, 0, 67, false);
				list = new List<object>();
				FinaleItems.Add(list);
				func_560(subTableScaleformHandle2, list, 0, 68, false);
				list = new List<object>();
				FinaleItems.Add(list);
				func_560(subTableScaleformHandle2, list, 64, 79, false);
				list = new List<object>();
				FinaleItems.Add(list);
				func_560(subTableScaleformHandle2, list, 64, 78, false);
				list = new List<object>();
				FinaleItems.Add(list);
				func_560(subTableScaleformHandle2, list, 64, 77, false);
				list = new List<object>();
				FinaleItems.Add(list);
				func_560(subTableScaleformHandle2, list, 64, 81, true);
				list = new List<object>();
				FinaleItems.Add(list);
				func_560(subTableScaleformHandle2, list, 64, 82, false);
				list = new List<object>();
				FinaleItems.Add(list);
				func_560(subTableScaleformHandle2, list, 64, 83, false);
				list = new List<object>();
				FinaleItems.Add(list);
				func_560(subTableScaleformHandle2, list, 64, 84, false);
				list = new List<object>();
				FinaleItems.Add(list);
				func_560(subTableScaleformHandle2, list, 64, 80, false);
				if (CayoPericoHeist.VehicleEnteranceMethod.Equals("KOSATKA"))
				{
					list = new List<object>();
					FinaleItems.Add(list);
					func_560(subTableScaleformHandle2, list, 63, 71, true);
				}
				if (CayoPericoHeist.VehicleEnteranceMethod.Equals("ALKONOST"))
				{
					list = new List<object>();
					FinaleItems.Add(list);
					func_560(subTableScaleformHandle2, list, 63, 72, true);
				}
				if (CayoPericoHeist.VehicleEnteranceMethod.Equals("VELUM"))
				{
					list = new List<object>();
					FinaleItems.Add(list);
					func_560(subTableScaleformHandle2, list, 63, 73, true);
				}
				if (CayoPericoHeist.VehicleEnteranceMethod.Equals("ANNIHILATOR2"))
				{
					list = new List<object>();
					FinaleItems.Add(list);
					func_560(subTableScaleformHandle2, list, 63, 74, true);
				}
				if (CayoPericoHeist.VehicleEnteranceMethod.Equals("PATROLBOAT"))
				{
					list = new List<object>();
					FinaleItems.Add(list);
					func_560(subTableScaleformHandle2, list, 63, 75, true);
				}
				if (CayoPericoHeist.VehicleEnteranceMethod.Equals("LONGFIN"))
				{
					list = new List<object>();
					FinaleItems.Add(list);
					func_560(subTableScaleformHandle2, list, 63, 76, true);
				}
				if (CayoPericoHeist.VehicleEnteranceMethod.Equals("VELUM"))
				{
					list = new List<object>();
					FinaleItems.Add(list);
					func_560(subTableScaleformHandle2, list, 64, 77, true);
				}
				if (CayoPericoHeist.VehicleEnteranceMethod.Equals("ALKONOST"))
				{
					list = new List<object>();
					FinaleItems.Add(list);
					func_560(subTableScaleformHandle2, list, 64, 78, true);
				}
				if (CayoPericoHeist.VehicleEnteranceMethod.Equals("LONGFIN") || CayoPericoHeist.VehicleEnteranceMethod.Equals("PATROLBOAT"))
				{
					int num7 = Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, 300);
					if (num7 < 100)
					{
						list = new List<object>();
						FinaleItems.Add(list);
						func_560(subTableScaleformHandle2, list, 64, 79, true);
					}
					if (num7 >= 100 && num7 < 200)
					{
						list = new List<object>();
						FinaleItems.Add(list);
						func_560(subTableScaleformHandle2, list, 64, 80, true);
					}
					if (num7 >= 200 && num7 <= 300)
					{
						list = new List<object>();
						FinaleItems.Add(list);
						func_560(subTableScaleformHandle2, list, 64, 81, true);
					}
				}
				if (CayoPericoHeist.VehicleEnteranceMethod.Equals("ANNIHILATOR2"))
				{
					int num8 = Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, 100);
					if (num8 < 50)
					{
						list = new List<object>();
						FinaleItems.Add(list);
						func_560(subTableScaleformHandle2, list, 64, 83, true);
					}
					else
					{
						list = new List<object>();
						FinaleItems.Add(list);
						func_560(subTableScaleformHandle2, list, 64, 82, true);
					}
				}
				if (CayoPericoHeist.Retrieved_CuttingTorch)
				{
					list = new List<object>();
					FinaleItems.Add(list);
					func_560(subTableScaleformHandle2, list, 65, 86, true);
				}
				else
				{
					list = new List<object>();
					FinaleItems.Add(list);
					func_560(subTableScaleformHandle2, list, 65, 85, false);
				}
				list = new List<object>();
				FinaleItems.Add(list);
				func_560(subTableScaleformHandle2, list, 66, 87, false);
				list = new List<object>();
				FinaleItems.Add(list);
				func_560(subTableScaleformHandle2, list, 66, 88, true);
				list = new List<object>();
				FinaleItems.Add(list);
				func_560(subTableScaleformHandle2, list, 66, 89, false);
				if (CayoPericoHeist.VehicleEnteranceMethod.Equals("VELUM"))
				{
					list = new List<object>();
					FinaleItems.Add(list);
					func_560(subTableScaleformHandle2, list, 66, 88, false);
					list = new List<object>();
					FinaleItems.Add(list);
					func_560(subTableScaleformHandle2, list, 66, 87, true);
				}
				if (CayoPericoHeist.VehicleEnteranceMethod.Equals("KOSATKA"))
				{
					list = new List<object>();
					FinaleItems.Add(list);
					func_560(subTableScaleformHandle2, list, 66, 90, true);
				}
				list = new List<object>();
				FinaleItems.Add(list);
				func_560(subTableScaleformHandle2, list, 67, 91, true);
				list = new List<object>();
				FinaleItems.Add(list);
				func_560(subTableScaleformHandle2, list, 67, 92, false);
				if (CayoPericoHeist.Current_Weapon_Loadout == 0)
				{
					list = new List<object>();
					FinaleItems.Add(list);
					func_560(subTableScaleformHandle2, list, 68, 93, true);
				}
				if (CayoPericoHeist.Current_Weapon_Loadout == 1)
				{
					list = new List<object>();
					FinaleItems.Add(list);
					func_560(subTableScaleformHandle2, list, 68, 94, true);
				}
				if (CayoPericoHeist.Current_Weapon_Loadout == 2)
				{
					list = new List<object>();
					FinaleItems.Add(list);
					func_560(subTableScaleformHandle2, list, 68, 95, true);
				}
				if (CayoPericoHeist.Current_Weapon_Loadout == 3)
				{
					list = new List<object>();
					FinaleItems.Add(list);
					func_560(subTableScaleformHandle2, list, 68, 96, true);
				}
				if (CayoPericoHeist.Current_Weapon_Loadout == 4)
				{
					list = new List<object>();
					FinaleItems.Add(list);
					func_560(subTableScaleformHandle2, list, 68, 97, true);
				}
				list = new List<object>();
				FinaleItems.Add(list);
				func_560(subTableScaleformHandle2, list, 69, 99, false);
				list = new List<object>();
				FinaleItems.Add(list);
				func_560(subTableScaleformHandle2, list, 69, 100, false);
				list = new List<object>();
				FinaleItems.Add(list);
				func_560(subTableScaleformHandle2, list, 69, 101, false);
				list = new List<object>();
				FinaleItems.Add(list);
				func_560(subTableScaleformHandle2, list, 69, 102, false);
				list = new List<object>();
				FinaleItems.Add(list);
				func_560(subTableScaleformHandle2, list, 69, 103, false);
				list = new List<object>();
				FinaleItems.Add(list);
				func_560(subTableScaleformHandle2, list, 69, 104, false);
				list = new List<object>();
				FinaleItems.Add(list);
				func_560(subTableScaleformHandle2, list, 70, 105, false);
				list = new List<object>();
				FinaleItems.Add(list);
				func_560(subTableScaleformHandle2, list, 70, 106, false);
				list = new List<object>();
				FinaleItems.Add(list);
				func_560(subTableScaleformHandle2, list, 70, 107, false);
				list = new List<object>();
				FinaleItems.Add(list);
				func_560(subTableScaleformHandle2, list, 70, 108, false);
				OStage = 2;
				break;
			}
			case 2:
			{
				Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, GetSubTableScaleformHandle(), "SET_MAP_VIEW");
				Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, BoolToInt(!ShowingFullMap));
				Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
				int num = 0;
				int num2 = 0;
				int num3 = 0;
				int subTableScaleformHandle = GetSubTableScaleformHandle();
				int mainTargetID = CayoPericoHeist.MainTargetID;
				int iParam = 0;
				int uParam = 1;
				if (CayoPericoHeist.MainTargetID < 0)
				{
					CayoPericoHeist.MainTargetID = 0;
				}
				if (CayoPericoHeist.MainTargetID > 4)
				{
					CayoPericoHeist.MainTargetID = 5;
				}
				if (CayoPericoHeist.CurrentMainTargetID == CayoPericoHeist.MainTargets.MadrazoFiles)
				{
					uParam = 4;
					iParam = 1100000;
				}
				if (CayoPericoHeist.CurrentMainTargetID == CayoPericoHeist.MainTargets.CashBonds)
				{
					uParam = 2;
					iParam = 1100000;
				}
				if (CayoPericoHeist.CurrentMainTargetID == CayoPericoHeist.MainTargets.SinsimitoTequila)
				{
					uParam = 0;
					iParam = 630000;
				}
				if (CayoPericoHeist.CurrentMainTargetID == CayoPericoHeist.MainTargets.RubyNecklace)
				{
					uParam = 1;
					iParam = 700000;
				}
				if (CayoPericoHeist.CurrentMainTargetID == CayoPericoHeist.MainTargets.PinkDiamond)
				{
					uParam = 3;
					iParam = 1300000;
				}
				if (CayoPericoHeist.CurrentMainTargetID == CayoPericoHeist.MainTargets.PantherStatue)
				{
					uParam = 5;
					iParam = 1900000;
				}
				func_852(GetSubTableScaleformHandle(), true, iParam, 784000, func_854(uParam), "H4_LOOT_ADD");
				func_851(GetSubTableScaleformHandle(), 99, 22000);
				func_851(GetSubTableScaleformHandle(), 100, 22000);
				func_851(GetSubTableScaleformHandle(), 101, 22000);
				func_851(GetSubTableScaleformHandle(), 102, 22000);
				func_851(GetSubTableScaleformHandle(), 103, 22000);
				func_851(GetSubTableScaleformHandle(), 104, 22000);
				func_811(GetSubTableScaleformHandle(), func_347(1));
				func_848(GetSubTableScaleformHandle(), "H4P_LIST_PREV");
				if (CayoTableSelection_Tab != 2)
				{
					SelectedLaunchButton = false;
					func_593(GetSubTableScaleformHandle(), false, func_594(0));
				}
				else
				{
					if (Function.Call<bool>(Hash.IS_CONTROL_JUST_PRESSED, new InputArgument[2] { 2, 34 }) && SelectedLaunchButton)
					{
						SelectedLaunchButton = false;
					}
					if (Function.Call<bool>(Hash.IS_CONTROL_JUST_PRESSED, new InputArgument[2] { 2, 35 }) && !SelectedLaunchButton)
					{
						SelectedLaunchButton = true;
					}
					if (SelectedLaunchButton)
					{
						func_593(GetSubTableScaleformHandle(), true, "CH_FINALE_LBS");
						if (Function.Call<bool>(Hash.IS_CONTROL_JUST_PRESSED, new InputArgument[2] { 2, 201 }))
						{
							if (CayoPericoHeist.GotWeaponLoadout && CayoPericoHeist.GotApproachVehicle && CayoPericoHeist.GotEquipmentLoadout)
							{
								CayoPericoHeist.FinaleLocked = false;
							}
							if (!CayoPericoHeist.FinaleLocked)
							{
								GUIBIX = -1;
								CayoPericoHeist.CayoPericoMissionSetup = CayoPericoHeist.GlobalOverrideMission.Finale;
								Notification.Show("Start Finale");
							}
							else if (CayoPericoHeist.MainTargetID < 2)
							{
								TextNotification("CHAR_PAVEL", "Pavel", "Finale Locked", "Finale Requires:\n1 Weapon Loadout & 1 Approach Vehicle. Cutting Torch/Demolition Charges, Fingerprint Cloner\nSafe Code.");
							}
							else
							{
								TextNotification("CHAR_PAVEL", "Pavel", "Finale Locked", "Finale Requires:\n1 Weapon Loadout & 1 Approach Vehicle. Cutting Torch/Demolition Charges, Fingerprint Cloner\nPlasma Cutter.");
							}
						}
					}
					else
					{
						func_593(GetSubTableScaleformHandle(), false, func_594(1));
					}
				}
				func_814(GetSubTableScaleformHandle(), true);
				func_817(GetSubTableScaleformHandle(), 105, 100, 100, 1, true);
				func_840(GetSubTableScaleformHandle(), 2);
				func_845(GetSubTableScaleformHandle(), false);
				if (InteractingWithCayoTableScreen)
				{
					Function.Call(Hash.ENABLE_CONTROL_ACTION, 2, 201, true);
					Function.Call(Hash.ENABLE_CONTROL_ACTION, 2, 237, true);
					Function.Call(Hash.ENABLE_CONTROL_ACTION, new InputArgument[3] { 2, 201, 1 });
					Function.Call(Hash.ENABLE_CONTROL_ACTION, new InputArgument[3] { 2, 237, 1 });
					if (Function.Call<bool>(Hash.IS_CONTROL_JUST_PRESSED, new InputArgument[2] { 2, 201 }) || Function.Call<bool>(Hash.IS_CONTROL_JUST_PRESSED, new InputArgument[2] { 2, 237 }))
					{
						GUICheckBool = false;
						Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, GetSubTableScaleformHandle(), "SET_INPUT_EVENT", 201f, -1082130432, -1082130432, -1082130432, -1082130432);
						Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, new InputArgument[2]
						{
							GetSubTableScaleformHandle(),
							"GET_CURRENT_SELECTION"
						});
						iLocal_1425 = Function.Call<int>(Hash.END_SCALEFORM_MOVIE_METHOD_RETURN_VALUE);
						Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, GetSubTableScaleformHandle(), "REQUEST_CURRENT_ROLLOVER");
						iLocal_1424 = Function.Call<int>(Hash.END_SCALEFORM_MOVIE_METHOD_RETURN_VALUE);
						SelectedRoll = iLocal_1424;
					}
					if (CayoTableSelection_Tab == 1 && CayoTableSelection_MenuID == -1)
					{
						GUICheckBool = true;
					}
					if (!GUICheckBool)
					{
						if (!Function.Call<bool>(Hash.IS_SCALEFORM_MOVIE_METHOD_RETURN_VALUE_READY, new InputArgument[1] { iLocal_1425 }))
						{
							if (CayoTableSelection_Tab == 1)
							{
								if (CayoTableSelection_MenuID != -1)
								{
									GUIBIX = Function.Call<int>(Hash.GET_SCALEFORM_MOVIE_METHOD_RETURN_VALUE_INT, iLocal_1425);
									LastGUIBix = GUIBIX;
									GUICheckBool = true;
									if (GUIBIX != 0 && GUIBIX != -1)
									{
										func_34(5);
									}
								}
								else
								{
									GUIBIX = -1;
								}
							}
							else
							{
								GUIBIX = Function.Call<int>(Hash.GET_SCALEFORM_MOVIE_METHOD_RETURN_VALUE_INT, iLocal_1425);
								LastGUIBix = GUIBIX;
								GUICheckBool = true;
								if (GUIBIX != 0 && GUIBIX != -1)
								{
									func_34(5);
								}
							}
						}
						if (Function.Call<bool>(Hash.IS_SCALEFORM_MOVIE_METHOD_RETURN_VALUE_READY, new InputArgument[1] { iLocal_1424 }))
						{
							SelectedRollover = Function.Call<int>(Hash.GET_SCALEFORM_MOVIE_METHOD_RETURN_VALUE_INT, new InputArgument[1] { iLocal_1424 });
						}
					}
					if (Function.Call<bool>(Hash.IS_CONTROL_JUST_PRESSED, new InputArgument[2] { 2, 201 }) || Function.Call<bool>(Hash.IS_CONTROL_JUST_PRESSED, new InputArgument[2] { 2, 237 }))
					{
						GUICheckBool = false;
						Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, GetSubTableScaleformHandle(), "SET_INPUT_EVENT", 201f, -1082130432, -1082130432, -1082130432, -1082130432);
						Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, GetSubTableScaleformHandle(), "REQUEST_CURRENT_SELECTION");
						iLocal_1425 = Function.Call<int>(Hash.END_SCALEFORM_MOVIE_METHOD_RETURN_VALUE);
						GUIAPP = iLocal_1425;
						Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, GetSubTableScaleformHandle(), "REQUEST_CURRENT_ROLLOVER");
						iLocal_1424 = Function.Call<int>(Hash.END_SCALEFORM_MOVIE_METHOD_RETURN_VALUE);
						SelectedRoll = iLocal_1424;
					}
					if (CayoTableSelection_Tab == 1 && CayoTableSelection_MenuID == -1)
					{
						GUICheckBool = true;
					}
					if (!GUICheckBool)
					{
						if (Function.Call<bool>(Hash.IS_SCALEFORM_MOVIE_METHOD_RETURN_VALUE_READY, new InputArgument[1] { iLocal_1425 }))
						{
							if (CayoTableSelection_Tab == 1)
							{
								if (CayoTableSelection_MenuID != -1)
								{
									GUIBIX = Function.Call<int>(Hash.GET_SCALEFORM_MOVIE_METHOD_RETURN_VALUE_INT, iLocal_1425);
									LastGUIBix = GUIBIX;
									GUICheckBool = true;
									if (GUIBIX != 0 && GUIBIX != -1)
									{
										func_34(5);
									}
								}
								else
								{
									GUIBIX = -1;
								}
							}
							else
							{
								GUIBIX = Function.Call<int>(Hash.GET_SCALEFORM_MOVIE_METHOD_RETURN_VALUE_INT, iLocal_1425);
								LastGUIBix = GUIBIX;
								GUICheckBool = true;
								if (GUIBIX != 0 && GUIBIX != -1)
								{
									func_34(5);
								}
							}
						}
						if (Function.Call<bool>(Hash.IS_SCALEFORM_MOVIE_METHOD_RETURN_VALUE_READY, new InputArgument[1] { iLocal_1424 }))
						{
							SelectedRollover = Function.Call<int>(Hash.GET_SCALEFORM_MOVIE_METHOD_RETURN_VALUE_INT, new InputArgument[1] { iLocal_1424 });
						}
					}
					Function.Call(Hash.ENABLE_CONTROL_ACTION, 2, 201, true);
					Function.Call(Hash.ENABLE_CONTROL_ACTION, 2, 237, true);
					Function.Call(Hash.ENABLE_CONTROL_ACTION, 2, 205, true);
					if (Function.Call<bool>(Hash.IS_CONTROL_JUST_PRESSED, 2, 205))
					{
						func_34(1);
						Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, GetSubTableScaleformHandle(), "SET_INPUT_EVENT", 206f, -1082130432, -1082130432, -1082130432, -1082130432);
						if (!CayoPericoHeist.PlayedIntelSetup)
						{
							CayoTableSelection_Tab = 0;
							GUICheckBool = false;
						}
						else if (CayoPericoHeist.FinaleLocked)
						{
							if (CayoTableSelection_Tab == 0)
							{
								CayoTableSelection_Tab = 1;
							}
							else if (CayoTableSelection_Tab == 1)
							{
								CayoTableSelection_Tab = 0;
							}
						}
						else if (CayoTableSelection_Tab == 0)
						{
							CayoTableSelection_Tab = 1;
						}
						else if (CayoTableSelection_Tab == 1)
						{
							CayoTableSelection_Tab = 2;
						}
						else if (CayoTableSelection_Tab == 2)
						{
							CayoTableSelection_Tab = 0;
						}
						CayoTableSelection_SelectionID = 0;
						CayoTableSelection_MenuID = -1;
						CayoTableSelection_MenuTabOpen = 0;
						CayoTableSelection_SubMenuTabOpen = -1;
						CayoTableSelection_SubSubMenuTabOpen = -1;
						if (CayoTableSelection_Tab == 0)
						{
							CurrentCayoSelection = "GATHER INTEL";
						}
						if (CayoTableSelection_Tab == 1)
						{
							CurrentCayoSelection = "APPROACH VEHICLES";
						}
						if (CayoTableSelection_Tab == 2)
						{
							CurrentCayoSelection = "FINALE";
						}
						return;
					}
					Function.Call(Hash.ENABLE_CONTROL_ACTION, 2, 206, true);
					if (Function.Call<bool>(Hash.IS_CONTROL_JUST_PRESSED, 2, 206))
					{
						func_34(1);
						Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, GetSubTableScaleformHandle(), "SET_INPUT_EVENT", 205f, -1082130432, -1082130432, -1082130432, -1082130432);
						if (!CayoPericoHeist.PlayedIntelSetup)
						{
							CayoTableSelection_Tab = 0;
							GUICheckBool = false;
						}
						else if (CayoPericoHeist.FinaleLocked)
						{
							if (CayoTableSelection_Tab == 0)
							{
								CayoTableSelection_Tab = 1;
							}
							else if (CayoTableSelection_Tab == 1)
							{
								CayoTableSelection_Tab = 0;
							}
						}
						else if (CayoTableSelection_Tab == 0)
						{
							CayoTableSelection_Tab = 2;
						}
						else if (CayoTableSelection_Tab == 2)
						{
							CayoTableSelection_Tab = 1;
						}
						else if (CayoTableSelection_Tab == 1)
						{
							CayoTableSelection_Tab = 0;
						}
						CayoTableSelection_SelectionID = 0;
						CayoTableSelection_MenuID = -1;
						CayoTableSelection_MenuTabOpen = 0;
						CayoTableSelection_SubMenuTabOpen = -1;
						CayoTableSelection_SubSubMenuTabOpen = -1;
						if (CayoTableSelection_Tab == 0)
						{
							CurrentCayoSelection = "GATHER INTEL";
						}
						if (CayoTableSelection_Tab == 1)
						{
							CurrentCayoSelection = "APPROACH VEHICLES";
						}
						if (CayoTableSelection_Tab == 2)
						{
							CurrentCayoSelection = "FINALE";
						}
						return;
					}
					if (!SelectedLaunchButton)
					{
						if (CayoTableSelection_Tab == 0)
						{
							if (CayoTableSelection_MenuTabOpen == 0)
							{
								Function.Call(Hash.ENABLE_CONTROL_ACTION, 2, 201, true);
								if (Function.Call<bool>(Hash.IS_CONTROL_JUST_PRESSED, 2, 201))
								{
									func_34(201);
									Script.Wait(0);
									GUIBIX = -1;
									CayoTableSelection_SelectionID = 0;
									CayoTableSelection_MenuTabOpen = 1;
									return;
								}
							}
							if (CayoTableSelection_MenuTabOpen == 1)
							{
								Function.Call(Hash.ENABLE_CONTROL_ACTION, 2, 201, true);
								if (Function.Call<bool>(Hash.IS_CONTROL_JUST_PRESSED, 2, 201))
								{
									func_34(201);
									Script.Wait(0);
									CayoTableSelection_SelectionID = 0;
									CayoTableSelection_SubMenuTabOpen = 1;
									return;
								}
								Function.Call(Hash.ENABLE_CONTROL_ACTION, 2, 35, true);
								if (Function.Call<bool>(Hash.IS_CONTROL_JUST_PRESSED, 2, 202))
								{
									func_34(202);
									Script.Wait(0);
									CayoTableSelection_SelectionID = 0;
									CayoTableSelection_MenuID = -1;
									CayoTableSelection_MenuTabOpen = 0;
									CayoTableSelection_SubMenuTabOpen = -1;
									CayoTableSelection_SubSubMenuTabOpen = -1;
								}
							}
							if (Function.Call<bool>(Hash.IS_CONTROL_JUST_PRESSED, 2, 32))
							{
								func_34(0);
								CayoTableSelection_SelectionID--;
								if (CayoTableSelection_SelectionID < 0)
								{
									CayoTableSelection_SelectionID = 5;
								}
								if (CayoTableSelection_SelectionID == 0)
								{
									CurrentCayoSelection = "GATHER INTEL";
								}
								if (CayoTableSelection_SelectionID == 1)
								{
									CurrentCayoSelection = "SECONDARY TARGETS";
								}
								if (CayoTableSelection_SelectionID == 2)
								{
									CurrentCayoSelection = "INFILTRATION POINTS";
								}
								if (CayoTableSelection_SelectionID == 3)
								{
									CurrentCayoSelection = "ESCAPE POINTS";
								}
								if (CayoTableSelection_SelectionID == 4)
								{
									CurrentCayoSelection = "COMPOUND ENTRY POINTS";
								}
								if (CayoTableSelection_SelectionID == 5)
								{
									CurrentCayoSelection = "POINTS OF INTEREST";
								}
							}
							if (Function.Call<bool>(Hash.IS_CONTROL_JUST_PRESSED, 2, 33))
							{
								func_34(0);
								CayoTableSelection_SelectionID++;
								if (CayoTableSelection_SelectionID > 5)
								{
									CayoTableSelection_SelectionID = 0;
								}
								if (CayoTableSelection_SelectionID == 0)
								{
									CurrentCayoSelection = "GATHER INTEL";
								}
								if (CayoTableSelection_SelectionID == 1)
								{
									CurrentCayoSelection = "SECONDARY TARGETS";
								}
								if (CayoTableSelection_SelectionID == 2)
								{
									CurrentCayoSelection = "INFILTRATION POINTS";
								}
								if (CayoTableSelection_SelectionID == 3)
								{
									CurrentCayoSelection = "ESCAPE POINTS";
								}
								if (CayoTableSelection_SelectionID == 4)
								{
									CurrentCayoSelection = "COMPOUND ENTRY POINTS";
								}
								if (CayoTableSelection_SelectionID == 5)
								{
									CurrentCayoSelection = "POINTS OF INTEREST";
								}
							}
						}
						if (CayoTableSelection_Tab == 1)
						{
							if (CayoTableSelection_MenuTabOpen == 0)
							{
								Function.Call(Hash.ENABLE_CONTROL_ACTION, 2, 201, true);
								if (Function.Call<bool>(Hash.IS_CONTROL_JUST_PRESSED, 2, 201))
								{
									func_34(201);
									Script.Wait(0);
									GUIBIX = -1;
									CayoTableSelection_MenuID = CayoTableSelection_SelectionID;
									CayoTableSelection_MenuTabOpen = 1;
									if (CayoTableSelection_MenuID == 0)
									{
										CurrentCayoSelection = "KOSATKA";
									}
									if (CayoTableSelection_MenuID == 1)
									{
										CurrentCayoSelection = "DEMOLITION CHARGES";
									}
									if (CayoTableSelection_MenuID == 2)
									{
										CurrentCayoSelection = "AGGRESSOR";
									}
									if (CayoTableSelection_MenuID == 3)
									{
										CurrentCayoSelection = "WEAPONS";
									}
									return;
								}
								if (Function.Call<bool>(Hash.IS_CONTROL_JUST_PRESSED, 2, 32))
								{
									func_34(0);
									CayoTableSelection_SelectionID--;
									if (CayoTableSelection_SelectionID < 0)
									{
										CayoTableSelection_SelectionID = 4;
									}
									if (CayoTableSelection_SelectionID == 0)
									{
										CurrentCayoSelection = "APPROACH VEHICLES";
									}
									if (CayoTableSelection_SelectionID == 1)
									{
										CurrentCayoSelection = "EQUIPMENT";
									}
									if (CayoTableSelection_SelectionID == 2)
									{
										CurrentCayoSelection = "WEAPON LOADOUT";
									}
									if (CayoTableSelection_SelectionID == 3)
									{
										CurrentCayoSelection = "DISRUPTION";
									}
								}
								if (Function.Call<bool>(Hash.IS_CONTROL_JUST_PRESSED, 2, 33))
								{
									func_34(0);
									CayoTableSelection_SelectionID++;
									if (CayoTableSelection_SelectionID > 4)
									{
										CayoTableSelection_SelectionID = 0;
									}
									if (CayoTableSelection_SelectionID < 0)
									{
										CayoTableSelection_SelectionID = 4;
									}
									if (CayoTableSelection_SelectionID == 0)
									{
										CurrentCayoSelection = "APPROACH VEHICLES";
									}
									if (CayoTableSelection_SelectionID == 1)
									{
										CurrentCayoSelection = "EQUIPMENT";
									}
									if (CayoTableSelection_SelectionID == 2)
									{
										CurrentCayoSelection = "WEAPON LOADOUT";
									}
									if (CayoTableSelection_SelectionID == 3)
									{
										CurrentCayoSelection = "DISRUPTION";
									}
								}
							}
							if (CayoTableSelection_MenuTabOpen == 1)
							{
								Function.Call(Hash.ENABLE_CONTROL_ACTION, 2, 201, true);
								if (Function.Call<bool>(Hash.IS_CONTROL_JUST_PRESSED, 2, 201))
								{
									func_34(201);
									Script.Wait(0);
									CayoTableSelection_SelectionID = CayoTableSelection_MenuID;
									CayoTableSelection_SubMenuTabOpen = 1;
									return;
								}
								Function.Call(Hash.ENABLE_CONTROL_ACTION, 2, 202, true);
								if (Function.Call<bool>(Hash.IS_CONTROL_JUST_PRESSED, 2, 202))
								{
									func_34(202);
									Script.Wait(0);
									CayoTableSelection_MenuID = -1;
									CayoTableSelection_MenuTabOpen = 0;
									CayoTableSelection_SubMenuTabOpen = -1;
									CayoTableSelection_SubSubMenuTabOpen = -1;
									CayoTableSelection_SelectionID = 0;
								}
								if (CayoTableSelection_MenuID == 0)
								{
									if (Function.Call<bool>(Hash.IS_CONTROL_JUST_PRESSED, 2, 32))
									{
										func_34(0);
										CayoTableSelection_SelectionID--;
										if (CayoTableSelection_SelectionID < 0)
										{
											CayoTableSelection_SelectionID = 5;
										}
										if (CayoTableSelection_SelectionID == 0)
										{
											CurrentCayoSelection = "KOSATKA";
										}
										if (CayoTableSelection_SelectionID == 1)
										{
											CurrentCayoSelection = "ALKONOST";
										}
										if (CayoTableSelection_SelectionID == 2)
										{
											CurrentCayoSelection = "VELUM";
										}
										if (CayoTableSelection_SelectionID == 3)
										{
											CurrentCayoSelection = "ANNIHILATOR STEALTH";
										}
										if (CayoTableSelection_SelectionID == 4)
										{
											CurrentCayoSelection = "PATROL BOAT";
										}
										if (CayoTableSelection_SelectionID == 5)
										{
											CurrentCayoSelection = "LONGFIN";
										}
									}
									if (Function.Call<bool>(Hash.IS_CONTROL_JUST_PRESSED, 2, 33))
									{
										func_34(0);
										CayoTableSelection_SelectionID++;
										if (CayoTableSelection_SelectionID > 5)
										{
											CayoTableSelection_SelectionID = 0;
										}
										if (CayoTableSelection_SelectionID == 0)
										{
											CurrentCayoSelection = "KOSATKA";
										}
										if (CayoTableSelection_SelectionID == 1)
										{
											CurrentCayoSelection = "ALKONOST";
										}
										if (CayoTableSelection_SelectionID == 2)
										{
											CurrentCayoSelection = "VELUM";
										}
										if (CayoTableSelection_SelectionID == 3)
										{
											CurrentCayoSelection = "ANNIHILATOR STEALTH";
										}
										if (CayoTableSelection_SelectionID == 4)
										{
											CurrentCayoSelection = "PATROL BOAT";
										}
										if (CayoTableSelection_SelectionID == 5)
										{
											CurrentCayoSelection = "LONGFIN";
										}
									}
								}
								if (CayoTableSelection_MenuID == 1)
								{
									if (Function.Call<bool>(Hash.IS_CONTROL_JUST_PRESSED, 2, 32))
									{
										func_34(0);
										CayoTableSelection_SelectionID--;
										if (CayoTableSelection_SelectionID < 0)
										{
											CayoTableSelection_SelectionID = 3;
										}
										if (CayoTableSelection_SelectionID == 0)
										{
											CurrentCayoSelection = "DEMOLITION CHARGES";
										}
										if (CayoTableSelection_SelectionID == 1)
										{
											CurrentCayoSelection = "PLASMA CUTTER";
										}
										if (CayoTableSelection_SelectionID == 2)
										{
											CurrentCayoSelection = "FINGERPRINT CLONER";
										}
										if (CayoTableSelection_SelectionID == 3)
										{
											CurrentCayoSelection = "CUTTING TORCH";
										}
									}
									if (Function.Call<bool>(Hash.IS_CONTROL_JUST_PRESSED, 2, 33))
									{
										func_34(0);
										CayoTableSelection_SelectionID++;
										if (CayoTableSelection_SelectionID > 3)
										{
											CayoTableSelection_SelectionID = 0;
										}
										if (CayoTableSelection_SelectionID == 0)
										{
											CurrentCayoSelection = "DEMOLITION CHARGES";
										}
										if (CayoTableSelection_SelectionID == 1)
										{
											CurrentCayoSelection = "PLASMA CUTTER";
										}
										if (CayoTableSelection_SelectionID == 2)
										{
											CurrentCayoSelection = "FINGERPRINT CLONER";
										}
										if (CayoTableSelection_SelectionID == 3)
										{
											CurrentCayoSelection = "CUTTING TORCH";
										}
									}
								}
								if (CayoTableSelection_MenuID == 2)
								{
									if (Function.Call<bool>(Hash.IS_CONTROL_JUST_PRESSED, 2, 32))
									{
										func_34(0);
										CayoTableSelection_SelectionID--;
										if (CayoTableSelection_SelectionID < 0)
										{
											CayoTableSelection_SelectionID = 5;
										}
										if (CayoTableSelection_SelectionID == 0)
										{
											CurrentCayoSelection = "AGGRESSOR";
										}
										if (CayoTableSelection_SelectionID == 1)
										{
											CurrentCayoSelection = "CONSPRIRATOR";
										}
										if (CayoTableSelection_SelectionID == 2)
										{
											CurrentCayoSelection = "CRACK SHOT";
										}
										if (CayoTableSelection_SelectionID == 3)
										{
											CurrentCayoSelection = "SABATOUR";
										}
										if (CayoTableSelection_SelectionID == 4)
										{
											CurrentCayoSelection = "MARKSMAN";
										}
										if (CayoTableSelection_SelectionID == 5)
										{
											CurrentCayoSelection = "SUPPRESSORS";
										}
									}
									if (Function.Call<bool>(Hash.IS_CONTROL_JUST_PRESSED, 2, 33))
									{
										func_34(0);
										CayoTableSelection_SelectionID++;
										if (CayoTableSelection_SelectionID > 5)
										{
											CayoTableSelection_SelectionID = 0;
										}
										if (CayoTableSelection_SelectionID == 0)
										{
											CurrentCayoSelection = "AGGRESSOR";
										}
										if (CayoTableSelection_SelectionID == 1)
										{
											CurrentCayoSelection = "CONSPRIRATOR";
										}
										if (CayoTableSelection_SelectionID == 2)
										{
											CurrentCayoSelection = "CRACK SHOT";
										}
										if (CayoTableSelection_SelectionID == 3)
										{
											CurrentCayoSelection = "SABATOUR";
										}
										if (CayoTableSelection_SelectionID == 4)
										{
											CurrentCayoSelection = "MARKSMAN";
										}
										if (CayoTableSelection_SelectionID == 5)
										{
											CurrentCayoSelection = "SUPPRESSORS";
										}
									}
								}
							}
							if (CayoTableSelection_MenuTabOpen == 1)
							{
								int cayoTableSelection_MenuID = CayoTableSelection_MenuID;
							}
						}
					}
					if (CayoTableSelection_Tab == 2)
					{
						if (CayoTableSelection_FinaleMenuOpenned == 0)
						{
							Function.Call(Hash.ENABLE_CONTROL_ACTION, 2, 201, true);
							if (Function.Call<bool>(Hash.IS_CONTROL_JUST_PRESSED, 2, 201))
							{
								func_34(201);
								Script.Wait(0);
								GUIBIX = -1;
								CayoTableSelection_FinaleMenuID = CayoTableSelection_FinaleSelectionID;
								CayoTableSelection_FinaleMenuOpenned = 1;
								return;
							}
						}
						if (CayoTableSelection_FinaleMenuOpenned == 1)
						{
							Function.Call(Hash.ENABLE_CONTROL_ACTION, 2, 202, true);
							if (Function.Call<bool>(Hash.IS_CONTROL_JUST_PRESSED, 2, 202))
							{
								func_34(201);
								Script.Wait(0);
								GUIBIX = -1;
								CayoTableSelection_FinaleSelectionID = CayoTableSelection_FinaleMenuID;
								CayoTableSelection_FinaleMenuOpenned = 0;
								CayoTableSelection_FinaleMenuID = -1;
								return;
							}
						}
					}
					if (!SelectedLaunchButton)
					{
						if (Function.Call<bool>(Hash.IS_CONTROL_JUST_PRESSED, 2, 32))
						{
							func_34(0);
							CayoTableSelection_FinaleSelectionID--;
						}
						if (Function.Call<bool>(Hash.IS_CONTROL_JUST_PRESSED, 2, 33))
						{
							func_34(0);
							CayoTableSelection_FinaleSelectionID++;
						}
						if (Function.Call<bool>(Hash.IS_CONTROL_JUST_PRESSED, 2, 34))
						{
							func_34(0);
							CayoTableSelection_FinaleItemSelection--;
						}
						if (Function.Call<bool>(Hash.IS_CONTROL_JUST_PRESSED, 2, 35))
						{
							func_34(0);
							CayoTableSelection_FinaleItemSelection++;
						}
					}
					func_547(GetSubTableScaleformHandle(), CayoTableSelection_FinaleItemSelection, true);
					func_547(GetSubTableScaleformHandle(), CayoTableSelection_FinaleItemSelection - 1, false);
					func_547(GetSubTableScaleformHandle(), CayoTableSelection_FinaleItemSelection + 1, false);
					if (Function.Call<bool>(Hash.IS_USING_CURSOR, 2))
					{
						Function.Call(Hash.ENABLE_CONTROL_ACTION, 2, 188, true);
						if (Function.Call<bool>(Hash.IS_CONTROL_JUST_PRESSED, 2, 188))
						{
							Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, GetSubTableScaleformHandle(), "SET_INPUT_EVENT", 301f, -1082130432, -1082130432, -1082130432, -1082130432);
						}
						Function.Call(Hash.ENABLE_CONTROL_ACTION, 2, 300, true);
						if (Function.Call<bool>(Hash.IS_CONTROL_JUST_PRESSED, 2, 300))
						{
							Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, GetSubTableScaleformHandle(), "SET_INPUT_EVENT", 300f, -1082130432, -1082130432, -1082130432, -1082130432);
						}
						if (Function.Call<bool>(Hash.IS_CONTROL_JUST_RELEASED, 2, 300))
						{
							Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, GetSubTableScaleformHandle(), "SET_INPUT_RELEASE_EVENT", 300f, -1082130432, -1082130432, -1082130432, -1082130432);
						}
						if (Function.Call<bool>(Hash.IS_CONTROL_JUST_RELEASED, 2, 301))
						{
							Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, GetSubTableScaleformHandle(), "SET_INPUT_RELEASE_EVENT", 301f, -1082130432, -1082130432, -1082130432, -1082130432);
						}
					}
					else
					{
						Function.Call(Hash.ENABLE_CONTROL_ACTION, 2, 32, true);
						if (Function.Call<bool>(Hash.IS_CONTROL_JUST_PRESSED, 2, 32))
						{
							Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, GetSubTableScaleformHandle(), "SET_INPUT_EVENT", 32f, -1082130432, -1082130432, -1082130432, -1082130432);
						}
						Function.Call(Hash.ENABLE_CONTROL_ACTION, 2, 33, true);
						if (Function.Call<bool>(Hash.IS_CONTROL_JUST_PRESSED, 2, 33))
						{
							Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, GetSubTableScaleformHandle(), "SET_INPUT_EVENT", 33f, -1082130432, -1082130432, -1082130432, -1082130432);
						}
						Function.Call(Hash.ENABLE_CONTROL_ACTION, 2, 34, true);
						if (Function.Call<bool>(Hash.IS_CONTROL_JUST_PRESSED, 2, 34))
						{
							Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, GetSubTableScaleformHandle(), "SET_INPUT_EVENT", 34f, -1082130432, -1082130432, -1082130432, -1082130432);
						}
						Function.Call(Hash.ENABLE_CONTROL_ACTION, 2, 35, true);
						if (Function.Call<bool>(Hash.IS_CONTROL_JUST_PRESSED, 2, 35))
						{
							Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, GetSubTableScaleformHandle(), "SET_INPUT_EVENT", 35f, -1082130432, -1082130432, -1082130432, -1082130432);
						}
					}
					Function.Call(Hash.ENABLE_CONTROL_ACTION, 2, 32, true);
					if (Function.Call<bool>(Hash.IS_CONTROL_JUST_PRESSED, 2, 32))
					{
						Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, GetSubTableScaleformHandle(), "SET_INPUT_EVENT", 188f, -1082130432, -1082130432, -1082130432, -1082130432);
					}
					Function.Call(Hash.ENABLE_CONTROL_ACTION, 2, 33, true);
					if (Function.Call<bool>(Hash.IS_CONTROL_JUST_PRESSED, 2, 33))
					{
						Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, GetSubTableScaleformHandle(), "SET_INPUT_EVENT", 187f, -1082130432, -1082130432, -1082130432, -1082130432);
					}
					Function.Call(Hash.ENABLE_CONTROL_ACTION, 2, 34, true);
					if (Function.Call<bool>(Hash.IS_CONTROL_JUST_PRESSED, 2, 34))
					{
						Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, GetSubTableScaleformHandle(), "SET_INPUT_EVENT", 189f, -1082130432, -1082130432, -1082130432, -1082130432);
					}
					Function.Call(Hash.ENABLE_CONTROL_ACTION, 2, 35, true);
					if (Function.Call<bool>(Hash.IS_CONTROL_JUST_PRESSED, 2, 35))
					{
						Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, GetSubTableScaleformHandle(), "SET_INPUT_EVENT", 190f, -1082130432, -1082130432, -1082130432, -1082130432);
					}
					Function.Call(Hash.ENABLE_CONTROL_ACTION, 2, 201, true);
					if (Function.Call<bool>(Hash.IS_CONTROL_JUST_PRESSED, 2, 201))
					{
						Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, GetSubTableScaleformHandle(), "SET_INPUT_EVENT", 201f, -1082130432, -1082130432, -1082130432, -1082130432);
					}
					Function.Call(Hash.ENABLE_CONTROL_ACTION, 2, 201, true);
					if (Function.Call<bool>(Hash.IS_CONTROL_JUST_PRESSED, 2, 201) || Function.Call<bool>(Hash.IS_CONTROL_JUST_PRESSED, 2, 237))
					{
						Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, GetSubTableScaleformHandle(), "SET_INPUT_EVENT", 201f, -1082130432, -1082130432, -1082130432, -1082130432);
					}
					Function.Call(Hash.ENABLE_CONTROL_ACTION, 2, 202, true);
					if (Function.Call<bool>(Hash.IS_CONTROL_PRESSED, 2, 202) || Function.Call<bool>(Hash.IS_CONTROL_PRESSED, 2, 237))
					{
						Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, GetSubTableScaleformHandle(), "SET_INPUT_EVENT", 202f, -1082130432, -1082130432, -1082130432, -1082130432);
					}
					Function.Call(Hash.ENABLE_CONTROL_ACTION, 2, 203, true);
					if (Function.Call<bool>(Hash.IS_CONTROL_JUST_PRESSED, 2, 203))
					{
						Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, GetSubTableScaleformHandle(), "SET_INPUT_EVENT", 203f, -1082130432, -1082130432, -1082130432, -1082130432);
					}
					Function.Call(Hash.ENABLE_CONTROL_ACTION, 2, 204, true);
					if (Function.Call<bool>(Hash.IS_CONTROL_JUST_PRESSED, 2, 204))
					{
						Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, GetSubTableScaleformHandle(), "SET_INPUT_EVENT", 204f, -1082130432, -1082130432, -1082130432, -1082130432);
					}
					func_579(GetSubTableScaleformHandle(), 187);
					func_579(GetSubTableScaleformHandle(), 188);
					func_579(GetSubTableScaleformHandle(), 189);
					func_579(GetSubTableScaleformHandle(), 190);
					if (Game.LastInputMethod == InputMethod.MouseAndKeyboard)
					{
						if (Game.IsControlJustPressed(Control.VehicleHandbrake))
						{
							ShowingFullMap = !ShowingFullMap;
						}
					}
					else if (Game.IsControlJustPressed(Control.FrontendX))
					{
						ShowingFullMap = !ShowingFullMap;
					}
					if (InteractingWithCayoTableScreen)
					{
						Game.DisableControlThisFrame(Control.FrontendPauseAlternate);
						Game.DisableControlThisFrame(Control.Phone);
						if (Game.IsControlJustPressed(Control.FrontendPauseAlternate))
						{
							SelectedLaunchButton = false;
							Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, GetSubTableScaleformHandle(), "SHOW_SCREEN");
							Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, 0);
							Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
							Script.Wait(500);
							if (CayoPericoHeist.GotWeaponLoadout && CayoPericoHeist.GotApproachVehicle && CayoPericoHeist.GotEquipmentLoadout)
							{
								CayoPericoHeist.FinaleLocked = false;
							}
							func_842(GetSubTableScaleformHandle());
							Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, GetSubTableScaleformHandle(), "SHOW_SCREEN");
							Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, 1);
							Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
							if (CayoPericoHeist.GotWeaponLoadout && CayoPericoHeist.GotApproachVehicle && CayoPericoHeist.GotEquipmentLoadout)
							{
								CayoPericoHeist.FinaleLocked = false;
							}
							func_842(GetSubTableScaleformHandle());
							InteractingWithCayoTableScreen = false;
							Game.Player.Character.IsVisible = true;
						}
					}
					Vector3 position = Game.Player.Character.Position;
					Prop prop = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, position.X, position.Y, position.Z, 5f, Function.Call<int>(Hash.GET_HASH_KEY, "ba_prop_battle_hacker_screen"), 0, 0, 0);
					if (prop != null && prop.Exists())
					{
						GetDistance(new Vector3(1561.11f, 384.75f, -49f), prop.Position);
						float num10 = 10f;
					}
				}
				ScreenPos = new Vector2(0.16f, 0.5f);
				ScreenSize = new Vector2(0.68f, 1f);
				Function.Call(Hash.SET_SCALEFORM_MOVIE_TO_USE_SUPER_LARGE_RT, GetSubTableScaleformHandle(), 1);
				Function.Call(Hash.SET_TEXT_RENDER_ID, OSF1);
				Function.Call(Hash.SET_SCRIPT_GFX_DRAW_ORDER, 4);
				Function.Call(Hash.SET_SCRIPT_GFX_DRAW_BEHIND_PAUSEMENU, 1);
				Function.Call(Hash.DRAW_SCALEFORM_MOVIE, GetSubTableScaleformHandle(), HeistIslandScaleformGetCentreX(), HeistIslandScaleformGetCentreY(), HeistIslandScaleformGetWidth(), HeistIslandScaleformGetHeight(), 255, 255, 255, 255, 0);
				Function.Call(Hash.SET_TEXT_RENDER_ID, Function.Call<int>(Hash.GET_DEFAULT_SCRIPT_RENDERTARGET_RENDER_ID));
				if (ShowingOverlay && GUIBIX == 1002)
				{
					func_524(GetSubTableScaleformHandle());
					GUIBIX = -1;
				}
				break;
			}
			}
			if (TableDebug)
			{
				drawText("OStage " + GUICheckBool + "/" + OStage + "/" + OSF1 + "/" + OSF2 + "/" + GetSubTableScaleformHandle() + "/ " + OSFScreen, 0.05f, 0.15f, 0.3f, 255, 255, 255);
				drawText("GUIBIX (" + GUIAPP + ")" + GUIBIX + "/" + LastGUIBix + "/" + iLocal_1425 + "\n", 0.05f, 0.18f, 0.3f, 255, 255, 255);
				drawText("Rollover (" + iLocal_1424 + ")" + SelectedRoll + "/" + SelectedRollover + "\n", 0.05f, 0.21f, 0.3f, 255, 255, 255);
				drawText("Selection " + CayoTableSelection_Tab + "/" + CayoTableSelection_MenuID + "/" + CayoTableSelection_SelectionID + "/" + CayoTableSelection_FinaleSelectionID, 0.05f, 0.25f, 0.3f, 255, 255, 255);
				drawText("MenuOpen " + CayoTableSelection_MenuTabOpen + "/" + CayoTableSelection_SubMenuTabOpen + "/" + CayoTableSelection_SubSubMenuTabOpen, 0.05f, 0.32f, 0.3f, 255, 255, 255);
				drawText("CurrentCayoSelection " + CurrentCayoSelection.ToString(), 0.05f, 0.28f, 0.3f, 255, 255, 255);
				drawText("CayoPericoMissionSetup {" + CayoPericoHeist.CayoPericoMissionSetup.ToString() + "} || {" + CayoPericoHeist.GlobalOverrideMissionID.ToString() + "}", 0.05f, 0.35f, 0.3f, 255, 255, 255);
				drawText("Setups Unlocked {" + CayoPericoHeist.PlayedIntelSetup + "}", 0.05f, 0.38f, 0.3f, 255, 255, 255);
				drawText("Finale Unlocked {" + CayoPericoHeist.GotWeaponLoadout + "} || {" + CayoPericoHeist.GotEquipmentLoadout + "} || {" + CayoPericoHeist.GotApproachVehicle + "}", 0.05f, 0.41f, 0.3f, 255, 255, 255);
				drawText("Finale Locked {" + CayoPericoHeist.FinaleLocked + "}", 0.05f, 0.44f, 0.3f, 255, 255, 255);
				drawText("Finale Items {" + CayoTableSelection_FinaleMenuID + "/" + CayoTableSelection_FinaleMenuOpenned + "/" + CayoTableSelection_FinaleSelectionID + "/" + CayoTableSelection_FinaleItemSelection + "}", 0.05f, 0.47f, 0.3f, 255, 255, 255);
				drawText("Finale Items {" + CayoPericoHeist.PlayedIntelSetup + "/" + CayoTableSelection_FinaleMenuOpenned + "/" + CayoTableSelection_FinaleSelectionID + "/" + CayoTableSelection_FinaleItemSelection + "}", 0.05f, 0.5f, 0.3f, 255, 255, 255);
			}
			if (CayoPericoHeist.GotWeaponLoadout && CayoPericoHeist.GotApproachVehicle && CayoPericoHeist.GotEquipmentLoadout)
			{
				CayoPericoHeist.FinaleLocked = false;
			}
			if (CayoTableSelection_Tab == 1)
			{
				if (CayoPericoHeist.CayoPericoMissionSetup == CayoPericoHeist.GlobalOverrideMission.None)
				{
					if (GUIBIX == 40)
					{
						GUIBIX = -1;
						Notification.Show("Starting Mission: ~b~Sonar Jammer~w~ Please Wait...");
						CayoPericoHeist.CayoPericoMissionSetup = CayoPericoHeist.GlobalOverrideMission.SonarJammer;
						SwitchOffCamera();
					}
					if (GUIBIX == 42)
					{
						GUIBIX = -1;
						Notification.Show("Starting Mission: ~b~Velum~w~ Please Wait...");
						CayoPericoHeist.CayoPericoMissionSetup = CayoPericoHeist.GlobalOverrideMission.Velum;
						SwitchOffCamera();
					}
					if (GUIBIX == 48)
					{
						GUIBIX = -1;
						Notification.Show("ANNIHILATOR STEALTH-VEHICLE (48)");
						CayoPericoHeist.CayoPericoMissionSetup = CayoPericoHeist.GlobalOverrideMission.AnnihilatorStealth;
						SwitchOffCamera();
					}
					if (GUIBIX == 49)
					{
						GUIBIX = -1;
						Notification.Show("Starting Mission: ~b~Annihilator Stealth - Pilot~w~ Please Wait...");
						CayoPericoHeist.CayoPericoMissionSetup = CayoPericoHeist.GlobalOverrideMission.Pilot;
						SwitchOffCamera();
					}
					if (GUIBIX == 44)
					{
						GUIBIX = -1;
						Notification.Show("Starting Mission: ~b~Patrol Boat~w~ Please Wait...");
						CayoPericoHeist.CayoPericoMissionSetup = CayoPericoHeist.GlobalOverrideMission.PatrolBoat;
						SwitchOffCamera();
					}
					if (GUIBIX == 45)
					{
						GUIBIX = -1;
						Notification.Show("Starting Mission: ~b~Longfin~w~ Please Wait...");
						CayoPericoHeist.CayoPericoMissionSetup = CayoPericoHeist.GlobalOverrideMission.Longfin;
						SwitchOffCamera();
					}
					if (GUIBIX == 46)
					{
						GUIBIX = -1;
						Notification.Show("Starting Mission: ~b~Alkonost~w~ Please Wait...");
						CayoPericoHeist.CayoPericoMissionSetup = CayoPericoHeist.GlobalOverrideMission.Alkonost;
						SwitchOffCamera();
					}
					if (GUIBIX == 47)
					{
						GUIBIX = -1;
						Notification.Show("Starting Mission: ~b~Alkonost - Pilot~w~ Please Wait...");
						CayoPericoHeist.CayoPericoMissionSetup = CayoPericoHeist.GlobalOverrideMission.SonarJammer;
						SwitchOffCamera();
					}
					if (GUIBIX == 50)
					{
						GUIBIX = -1;
						Notification.Show("Starting Mission: ~b~Demolition Charges~w~ Please Wait...");
						CayoPericoHeist.CayoPericoMissionSetup = CayoPericoHeist.GlobalOverrideMission.C4;
						SwitchOffCamera();
					}
					if (GUIBIX == 51)
					{
						GUIBIX = -1;
						Notification.Show("Starting Mission: ~b~Plasma Cutter/Safe Codes~w~ Please Wait...");
						CayoPericoHeist.CayoPericoMissionSetup = CayoPericoHeist.GlobalOverrideMission.PlasmaCutter;
						SwitchOffCamera();
					}
				}
				if (GUIBIX == 2000 && CayoPericoHeist.CayoPericoMissionSetup == CayoPericoHeist.GlobalOverrideMission.None)
				{
					if (CayoPericoHeist.MainTargetID == 0 && CayoPericoHeist.MainTargetID == 1)
					{
						GUIBIX = -1;
						ShowingOverlay = true;
						func_597(GetSubTableScaleformHandle(), "Test Case 1", "Test Case 2", func_599(), "");
					}
					else
					{
						GUIBIX = -1;
						Notification.Show("Starting Mission: ~b~Plasma Cutter~w~ Please Wait...");
						CayoPericoHeist.CayoPericoMissionSetup = CayoPericoHeist.GlobalOverrideMission.PlasmaCutter;
						SwitchOffCamera();
					}
				}
				if (GUIBIX == 1000 && CayoPericoHeist.CayoPericoMissionSetup == CayoPericoHeist.GlobalOverrideMission.None)
				{
					if (CayoPericoHeist.MainTargetID != 0 && CayoPericoHeist.MainTargetID != 1)
					{
						GUIBIX = -1;
						ShowingOverlay = true;
						func_597(GetSubTableScaleformHandle(), "Test Case 1", "Test Case 2", func_599(), "");
					}
					else
					{
						GUIBIX = -1;
						Notification.Show("Starting Mission: ~b~Safe Codes~w~ Please Wait...");
						CayoPericoHeist.CayoPericoMissionSetup = CayoPericoHeist.GlobalOverrideMission.SafeCodes;
						SwitchOffCamera();
					}
				}
				if (CayoPericoHeist.CayoPericoMissionSetup == CayoPericoHeist.GlobalOverrideMission.None)
				{
					if (GUIBIX == 52)
					{
						GUIBIX = -1;
						Notification.Show("Starting Mission: ~b~Fingerprint Cloner~w~ Please Wait...");
						CayoPericoHeist.CayoPericoMissionSetup = CayoPericoHeist.GlobalOverrideMission.FingerprintCloner;
						SwitchOffCamera();
					}
					if (GUIBIX == 53)
					{
						GUIBIX = -1;
						Notification.Show("Starting Mission: ~b~Cutting Torch~w~ Please Wait...");
						CayoPericoHeist.CayoPericoMissionSetup = CayoPericoHeist.GlobalOverrideMission.CuttingTorch;
						SwitchOffCamera();
					}
					if (GUIBIX == 54)
					{
						GUIBIX = -1;
						Notification.Show("Starting Mission: ~b~Weapons - Aggressor~w~ Please Wait...");
						CayoPericoHeist.CayoPericoMissionSetup = CayoPericoHeist.GlobalOverrideMission.Weapons;
						CayoPericoHeist.Current_Weapon_Loadout = 0;
						CayoPericoHeist.Config.SetValue("CAYOPERICOHEIST", "Player_Finale_Selected_Loadout", CayoPericoHeist.Current_Weapon_Loadout);
						CayoPericoHeist.Config.Save();
						SwitchOffCamera();
					}
					if (GUIBIX == 55)
					{
						GUIBIX = -1;
						Notification.Show("Starting Mission: ~b~Weapons - Conspirator~w~ Please Wait...");
						CayoPericoHeist.CayoPericoMissionSetup = CayoPericoHeist.GlobalOverrideMission.Weapons;
						CayoPericoHeist.Current_Weapon_Loadout = 1;
						CayoPericoHeist.Config.SetValue("CAYOPERICOHEIST", "Player_Finale_Selected_Loadout", CayoPericoHeist.Current_Weapon_Loadout);
						CayoPericoHeist.Config.Save();
						SwitchOffCamera();
					}
					if (GUIBIX == 56)
					{
						GUIBIX = -1;
						Notification.Show("Starting Mission: ~b~Weapons - Crack Shot~w~ Please Wait...");
						CayoPericoHeist.CayoPericoMissionSetup = CayoPericoHeist.GlobalOverrideMission.Weapons;
						CayoPericoHeist.Current_Weapon_Loadout = 2;
						CayoPericoHeist.Config.SetValue("CAYOPERICOHEIST", "Player_Finale_Selected_Loadout", CayoPericoHeist.Current_Weapon_Loadout);
						CayoPericoHeist.Config.Save();
						SwitchOffCamera();
					}
					if (GUIBIX == 57)
					{
						GUIBIX = -1;
						Notification.Show("Starting Mission: ~b~Weapons - Sabateur~w~ Please Wait...");
						CayoPericoHeist.CayoPericoMissionSetup = CayoPericoHeist.GlobalOverrideMission.Weapons;
						CayoPericoHeist.Current_Weapon_Loadout = 3;
						CayoPericoHeist.Config.SetValue("CAYOPERICOHEIST", "Player_Finale_Selected_Loadout", CayoPericoHeist.Current_Weapon_Loadout);
						CayoPericoHeist.Config.Save();
						SwitchOffCamera();
					}
					if (GUIBIX == 58)
					{
						GUIBIX = -1;
						Notification.Show("Starting Mission: ~b~Weapons - Marksman~w~ Please Wait...");
						CayoPericoHeist.CayoPericoMissionSetup = CayoPericoHeist.GlobalOverrideMission.Weapons;
						CayoPericoHeist.Current_Weapon_Loadout = 4;
						CayoPericoHeist.Config.SetValue("CAYOPERICOHEIST", "Player_Finale_Selected_Loadout", CayoPericoHeist.Current_Weapon_Loadout);
						CayoPericoHeist.Config.Save();
						SwitchOffCamera();
					}
					if (GUIBIX == 59)
					{
						GUIBIX = -1;
						Notification.Show("Starting Mission: ~b~Weapons - Suppressors~w~ Please Wait...");
						CayoPericoHeist.CayoPericoMissionSetup = CayoPericoHeist.GlobalOverrideMission.Weapons;
						SwitchOffCamera();
					}
				}
			}
			if (CayoTableSelection_Tab == 0 && CayoPericoHeist.CayoPericoMissionSetup == CayoPericoHeist.GlobalOverrideMission.None && GUIBIX == 1)
			{
				GUIBIX = -1;
				Notification.Show("Starting Mission: ~b~Intel~w~ Please Wait...");
				CayoPericoHeist.CayoPericoMissionSetup = CayoPericoHeist.GlobalOverrideMission.IntelScout;
				SwitchOffCamera();
			}
			if (InteractingWithCayoTableScreen)
			{
				if (Game.LastInputMethod == InputMethod.MouseAndKeyboard)
				{
					Scaleform scaleform = new Scaleform("instructional_buttons");
					scaleform.CallFunction("CLEAR_ALL");
					scaleform.CallFunction("TOGGLE_MOUSE_BUTTONS", 0);
					scaleform.CallFunction("CREATE_CONTAINER");
					scaleform.CallFunction("SET_DATA_SLOT", 3, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 76, 0), " Map");
					scaleform.CallFunction("SET_DATA_SLOT", 0, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 201, 0), " Select");
					scaleform.CallFunction("SET_DATA_SLOT", 1, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 194, 0), " Back");
					scaleform.CallFunction("SET_DATA_SLOT", 2, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 200, 0), " Quit");
					scaleform.CallFunction("SET_DATA_SLOT", 4, "~INPUT_FRONTEND_LB~", "");
					scaleform.CallFunction("SET_DATA_SLOT", 5, "~INPUT_FRONTEND_RB~", "Screens");
					scaleform.CallFunction("SET_DATA_SLOT", 6, "~INPUT_MOVE_UP_ONLY~", "");
					scaleform.CallFunction("SET_DATA_SLOT", 7, "~INPUT_MOVE_LEFT_ONLY~", "");
					scaleform.CallFunction("SET_DATA_SLOT", 8, "~INPUT_MOVE_DOWN_ONLY~", "");
					scaleform.CallFunction("SET_DATA_SLOT", 9, "~INPUT_MOVE_RIGHT_ONLY~", " Navigate");
					scaleform.CallFunction("SET_DATA_SLOT", 9, "~INPUT_MOVE_RIGHT_ONLY~", " Navigate");
					scaleform.CallFunction("SET_PADDING", 1f, 1f, 1f, 1f);
					scaleform.CallFunction("SET_MAX_WIDTH", 0.75f);
					scaleform.CallFunction("DRAW_INSTRUCTIONAL_BUTTONS", -1);
					scaleform.Render2D();
				}
				else
				{
					Scaleform scaleform2 = new Scaleform("instructional_buttons");
					scaleform2.CallFunction("CLEAR_ALL");
					scaleform2.CallFunction("TOGGLE_MOUSE_BUTTONS", 0);
					scaleform2.CallFunction("CREATE_CONTAINER");
					scaleform2.CallFunction("SET_DATA_SLOT", 3, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 203, 0), " Map");
					scaleform2.CallFunction("SET_DATA_SLOT", 0, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 201, 0), " Select");
					scaleform2.CallFunction("SET_DATA_SLOT", 1, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 194, 0), " Back");
					scaleform2.CallFunction("SET_DATA_SLOT", 2, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 200, 0), " Quit");
					scaleform2.CallFunction("SET_DATA_SLOT", 4, "~INPUT_FRONTEND_LB~", "");
					scaleform2.CallFunction("SET_DATA_SLOT", 5, "~INPUT_FRONTEND_RB~", "Screens");
					scaleform2.CallFunction("SET_DATA_SLOT", 6, "~INPUT_MOVE_UP_ONLY~", "");
					scaleform2.CallFunction("SET_DATA_SLOT", 7, "~INPUT_MOVE_LEFT_ONLY~", "");
					scaleform2.CallFunction("SET_DATA_SLOT", 8, "~INPUT_MOVE_DOWN_ONLY~", "");
					scaleform2.CallFunction("SET_DATA_SLOT", 9, "~INPUT_MOVE_RIGHT_ONLY~", " Navigate");
					scaleform2.CallFunction("SET_DATA_SLOT", 9, "~INPUT_MOVE_RIGHT_ONLY~", " Navigate");
					scaleform2.CallFunction("SET_PADDING", 1f, 1f, 1f, 1f);
					scaleform2.CallFunction("SET_MAX_WIDTH", 0.75f);
					scaleform2.CallFunction("DRAW_INSTRUCTIONAL_BUTTONS", -1);
					scaleform2.Render2D();
				}
			}
			else if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(2716.13f, -358.47f, -53.53f), ">", 10f))
			{
				Vector3 position2 = Game.Player.Character.Position;
				Prop prop2 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, position2.X, position2.Y, position2.Z, 3f, Function.Call<int>(Hash.GET_HASH_KEY, "ba_prop_battle_hacker_screen"), 0, 0, 0);
				if (prop2 != null)
				{
					prop2.Exists();
				}
			}
		}

		public float GetDistance(Vector3 _1, Vector3 _2)
		{
			return _1.DistanceTo(_2);
		}

		public static bool GetDistance_VDIST2(Vector3 _1, Vector3 _2, string Sign, float Radius)
		{
			bool result = false;
			float num = Radius * Radius;
			float num2 = Function.Call<float>(Hash.VDIST2, _1.X, _1.Y, _1.Z, _2.X, _2.Y, _2.Z);
			if (Sign.Equals("<=") && num2 <= num)
			{
				result = true;
			}
			if (Sign.Equals("<") && num2 < num)
			{
				result = true;
			}
			if (Sign.Equals(">=") && num2 >= num)
			{
				result = true;
			}
			if (Sign.Equals(">") && num2 > num)
			{
				result = true;
			}
			return result;
		}

		public CayoPericoSubTable()
		{
			base.Tick += onTick;
			base.Aborted += OnShutdown;
		}

		public void OnShutdown(object sender, EventArgs e)
		{
			if (SubTableScale != null)
			{
				SubTableScale.Dispose();
			}
			Game.Player.Character.IsVisible = true;
			if (ScreenCamera != null)
			{
				Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 0, 3000, 1, 1);
				ScreenCamera.IsActive = false;
				ScreenCamera.Delete();
				Game.Player.Character.IsPositionFrozen = false;
				Game.Player.SetControlState(true);
				ScreenCamera = null;
			}
			if (TableScreen != null)
			{
				TableScreen.Delete();
			}
			if (TableScreen2 != null)
			{
				TableScreen2.Delete();
			}
			if (Egg != null)
			{
				Egg.Delete();
			}
		}

		[DllImport("TrainerV.asi", CallingConvention = CallingConvention.Cdecl)]
		public static extern void disablempmap();

		public void onTick(object sender, EventArgs e)
		{
			try
			{
				disablempmap();
			}
			catch
			{
			}
			if (Function.Call<int>(Hash.UPDATE_ONSCREEN_KEYBOARD) == 0)
			{
				LastInputtedCommand = "";
				InputtedCommandStage = 0;
			}
			if (Function.Call<int>(Hash.UPDATE_ONSCREEN_KEYBOARD) == 1 && InputtedCommandStage == 0)
			{
				LastInputtedCommand = Function.Call<string>(Hash.GET_ONSCREEN_KEYBOARD_RESULT);
				InputtedCommandStage = 1;
				Function.Call(Hash.FORCE_CLOSE_TEXT_INPUT_BOX);
				if (LastInputtedCommand.Equals("debug_subtable"))
				{
					LastInputtedCommand = "";
					TableDebug = !TableDebug;
				}
			}
			if (!KosatkaScript.SittinginSeat1 && !KosatkaScript.SittinginSeat2 && !KosatkaScript.SittinginSeat && KosatkaScript.IsInInterior && Game.IsControlJustPressed(Control.Context) && !InteractingWithCayoTableScreen)
			{
				Vector3 position = Game.Player.Character.Position;
				bool flag = false;
				Prop prop = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, position.X, position.Y, position.Z, 3f, Function.Call<int>(Hash.GET_HASH_KEY, "ba_prop_battle_hacker_screen"), 0, 0, 0);
				if (prop != null && prop.Exists())
				{
					flag = true;
				}
				if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(1561.11f, 384.75f, -49f), "<", 4f))
				{
					flag = true;
				}
				if (flag)
				{
					if (Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, GetSubTableScaleformHandle()))
					{
						GUICheckBool = false;
						SelectedLaunchButton = false;
						CayoTableSelection_Tab = 0;
						CurrentCayoSelection = "GATHER INTEL";
						CayoTableSelection_Tab = 0;
						CayoTableSelection_MenuID = -1;
						CayoTableSelection_SelectionID = 0;
						if (CayoPericoHeist.GotWeaponLoadout && CayoPericoHeist.GotApproachVehicle && CayoPericoHeist.GotEquipmentLoadout)
						{
							CayoPericoHeist.FinaleLocked = false;
						}
						func_842(GetSubTableScaleformHandle());
						Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, GetSubTableScaleformHandle(), "SHOW_SCREEN");
						Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, 0);
						Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
						func_842(GetSubTableScaleformHandle());
						Script.Wait(500);
						if (CayoPericoHeist.GotWeaponLoadout && CayoPericoHeist.GotApproachVehicle && CayoPericoHeist.GotEquipmentLoadout)
						{
							CayoPericoHeist.FinaleLocked = false;
						}
						func_842(GetSubTableScaleformHandle());
						Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, GetSubTableScaleformHandle(), "SHOW_SCREEN");
						Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, 1);
						Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
						if (CayoPericoHeist.GotWeaponLoadout && CayoPericoHeist.GotApproachVehicle && CayoPericoHeist.GotEquipmentLoadout)
						{
							CayoPericoHeist.FinaleLocked = false;
						}
						func_842(GetSubTableScaleformHandle());
					}
					Game.Player.Character.IsVisible = false;
					InteractingWithCayoTableScreen = true;
					Script.Wait(1);
					Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, GetSubTableScaleformHandle(), "SET_INPUT_EVENT", 206f, -1082130432, -1082130432, -1082130432, -1082130432);
				}
			}
			if (!KosatkaScript.IsInInterior)
			{
				return;
			}
			Vector3 position2 = Game.Player.Character.Position;
			bool flag2 = false;
			if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(1561.11f, 384.75f, -49f), "<", 10f))
			{
				flag2 = true;
			}
			if (!flag2)
			{
				return;
			}
			CreateTableScreen();
			if (InteractingWithCayoTableScreen)
			{
				if (ScreenCamera == null)
				{
					ScreenCamera = World.CreateCamera(new Vector3(1561.1604f, 384.8204f, -48.5135f), new Vector3(-89.5003f, -0.204f, 180f), GameplayCamera.FieldOfView);
					ScreenCamera.IsActive = true;
					ScreenCamera.FarClip = 2000f;
					Function.Call(Hash.RENDER_SCRIPT_CAMS, 1, 1, 3000, 1, 0);
				}
				if (Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, GetSubTableScaleformHandle()))
				{
					func_633(GetSubTableScaleformHandle(), 2000, BoolToInt(false));
					func_633(GetSubTableScaleformHandle(), 40, BoolToInt(CayoPericoHeist.Retrieved_SonarJammer));
					func_633(GetSubTableScaleformHandle(), 41, BoolToInt(CayoPericoHeist.Retrieved_Pilot && CayoPericoHeist.Retrieved_Alkonost));
					func_633(GetSubTableScaleformHandle(), 47, BoolToInt(CayoPericoHeist.Retrieved_Pilot));
					func_633(GetSubTableScaleformHandle(), 42, BoolToInt(CayoPericoHeist.Retrieved_Velum));
					func_633(GetSubTableScaleformHandle(), 43, BoolToInt(CayoPericoHeist.Retrieved_Pilot && CayoPericoHeist.Retrieved_AnnihilatorStealth));
					func_633(GetSubTableScaleformHandle(), 49, BoolToInt(CayoPericoHeist.Retrieved_Pilot));
					func_633(GetSubTableScaleformHandle(), 44, BoolToInt(CayoPericoHeist.Retrieved_PatrolBoat));
					func_633(GetSubTableScaleformHandle(), 45, BoolToInt(CayoPericoHeist.Retrieved_Longfin));
					func_633(GetSubTableScaleformHandle(), 50, BoolToInt(CayoPericoHeist.Retrieved_C4));
					func_633(GetSubTableScaleformHandle(), 1000, BoolToInt(CayoPericoHeist.Retrieved_SafeCodes));
					func_633(GetSubTableScaleformHandle(), 2000, BoolToInt(CayoPericoHeist.Retrieved_PlasmaCutter));
					func_633(GetSubTableScaleformHandle(), 52, BoolToInt(CayoPericoHeist.Retrieved_FingerprintCloner));
					func_633(GetSubTableScaleformHandle(), 53, BoolToInt(CayoPericoHeist.Retrieved_CuttingTorch));
					func_633(GetSubTableScaleformHandle(), 54, 0);
					func_633(GetSubTableScaleformHandle(), 55, 0);
					func_633(GetSubTableScaleformHandle(), 56, 0);
					func_633(GetSubTableScaleformHandle(), 57, 0);
					func_633(GetSubTableScaleformHandle(), 58, 0);
					if (CayoPericoHeist.Current_Weapon_Loadout == 0)
					{
						func_633(GetSubTableScaleformHandle(), 54, 1);
					}
					if (CayoPericoHeist.Current_Weapon_Loadout == 1)
					{
						func_633(GetSubTableScaleformHandle(), 55, 1);
					}
					if (CayoPericoHeist.Current_Weapon_Loadout == 2)
					{
						func_633(GetSubTableScaleformHandle(), 56, 1);
					}
					if (CayoPericoHeist.Current_Weapon_Loadout == 3)
					{
						func_633(GetSubTableScaleformHandle(), 57, 1);
					}
					if (CayoPericoHeist.Current_Weapon_Loadout == 4)
					{
						func_633(GetSubTableScaleformHandle(), 58, 1);
					}
					CayoPericoHeist.GotApproachVehicle = CayoPericoHeist.Retrieved_Longfin || CayoPericoHeist.Retrieved_PatrolBoat || (CayoPericoHeist.Retrieved_Pilot && CayoPericoHeist.Retrieved_AnnihilatorStealth) || CayoPericoHeist.Retrieved_Velum || (CayoPericoHeist.Retrieved_Pilot && CayoPericoHeist.Retrieved_Alkonost) || CayoPericoHeist.Retrieved_SonarJammer;
					func_633(GetSubTableScaleformHandle(), 36, BoolToInt(CayoPericoHeist.GotApproachVehicle));
					CayoPericoHeist.GotEquipmentLoadout = (CayoPericoHeist.Retrieved_CuttingTorch || CayoPericoHeist.Retrieved_C4) && CayoPericoHeist.Retrieved_FingerprintCloner && (CayoPericoHeist.Retrieved_SafeCodes || CayoPericoHeist.Retrieved_PlasmaCutter);
					func_633(GetSubTableScaleformHandle(), 37, BoolToInt(CayoPericoHeist.GotEquipmentLoadout));
					CayoPericoHeist.GotWeaponLoadout = CayoPericoHeist.Retrieved_Weapons;
					func_633(GetSubTableScaleformHandle(), 38, BoolToInt(CayoPericoHeist.GotWeaponLoadout));
				}
				Game.Player.SetControlState(false);
			}
			else if (ScreenCamera != null)
			{
				Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 1, 3000, 1, 0);
				ScreenCamera.IsActive = false;
				ScreenCamera.Delete();
				Game.Player.Character.IsPositionFrozen = false;
				Game.Player.SetControlState(true);
				ScreenCamera = null;
				Game.Player.SetControlState(true);
			}
		}

		public static void SwitchOffCamera()
		{
			if (ScreenCamera != null)
			{
				Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 1, 3000, 1, 0);
				ScreenCamera.IsActive = false;
				ScreenCamera.Delete();
				Game.Player.Character.IsPositionFrozen = false;
				Game.Player.SetControlState(true);
				ScreenCamera = null;
				Game.Player.SetControlState(true);
			}
		}
	}
}
