# FFXIV-Packet-Analyzer

This is an ACT plugin for analyzing network packets, which supports parsing known packet arguments and dumping the parsed values along with raw packet data in human readable format.

## Supported Segments 

* `3` - IPC
* `8` - ServerKeepAlive

## Supported IPC Types

* `0065` - GroupMessage
* `00F7` - PublicMessage
* `0141` - AddStatusEffect
* `0143` - ActorControl143 (Fate & Currency)
* `0151` - StatusEffectList
* `0154` - Ability1
* `0157` - Ability8
* `0158` - Ability16
* `0159` - Ability24
* `015A` - Ability32
* `0196` - ItemInit
* `019B` - ItemSimple
* `019E` - ItemChange

## LICENSE
 
[GPL v3](LICENSE)

FINAL FANTASY, FINAL FANTASY XIV, FFXIV, SQUARE ENIX, and the SQUARE ENIX logo are registered trademarks or trademarks of Square Enix Holdings Co., Ltd.
