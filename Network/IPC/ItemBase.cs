using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacketAnalyzer.Network.IPC
{
    enum ItemLocation : ushort
    {
        Inventory0 = 0,
        Inventory1,
        Inventory2,
        Inventory3,
        ArmouryEquipped = 0x03e8,
        Currency = 0x07d0,
        Crystal = 0x07d1,
        SoulCrystal = 0x0d48,
        ArmouryMainHand = 0x0dac,
        ArmouryOffHand = 0x0c80,
        ArmouryHead,
        ArmouryBody,
        ArmouryHands,
        ArmouryWaist,
        ArmouryLegs,
        ArmouryFeet,
        ArmouryEars,
        ArmouryNeck,
        ArmouryWrists,
        ArmouryRightRing,
        ArmouryLeftRing,
    }

    enum ItemArmouryPosition : ushort
    {
        MainHand,
        OffHand,
        Head,
        Body,
        Hands,
        Waist,
        Legs,
        Feet,
        Ears,
        Neck,
        Wrists,
        RightRing,
        LeftRing,
        SoulCrystal
    }
}
