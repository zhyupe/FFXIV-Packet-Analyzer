# FFXIV-Packet-Analyzer

This is an ACT plugin for analyzing network packets, which supports parsing known packet arguments and dumping the parsed values along with raw packet data in human readable format.

## Half-Deprecated

This plugin is half-deprecated in favor of [FFXIV-Packet-Dissector](https://github.com/zhyupe/FFXIV-Packet-Dissector/), a set of plugins for Wireshark, for better experience of packet dumping and analyzing  along with the ability to dump outbound (client to server) packets which is broken in Machina. In addition, this ACT plugin processes the packets much slower than the Node.js scripts I locally used, and I can't figure out how to optimize as I'm not a professional in C#.

However, as the dissector cannot split segments in a single bundle, it's quite difficult to filter out a specified IPC packet. So this tool is still valuable for that job. But there might be another project to handle this problem properly.

## Supported Segments 

* `3` - IPC
* `8` - ServerKeepAlive

## Supported IPC Types

* `0065` - GroupMessage
* `00F7` - PublicMessage
* `010C` - Announcement
* `013F` - CompanyBoard
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
