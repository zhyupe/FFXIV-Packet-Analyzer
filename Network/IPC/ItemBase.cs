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
        ArmouryEquipped = 1000,
        Currency = 2000,
        Crystal,
        ArmouryOffHand = 3200,
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
        ArmouryRing = 3300,
        SoulCrystal = 3400,
        ArmouryMainHand = 3500,
        Saddlebag0 = 4000,
        Saddlebag1,
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
