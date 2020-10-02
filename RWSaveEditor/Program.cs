using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RWSaveEditor {
    class Program {
        static void Main(string[] args) {
            Save save = new Save();

            string saveFile = File.ReadAllText("sav.txt");
            saveFile = saveFile.Substring(32);

            string[] saveSplit = Regex.Split(saveFile, "<svA>");
            List<string[]> sa = new List<string[]>();

            for (int i = 0; i < saveSplit.Length; i++) {
                string[] split = Regex.Split(saveSplit[i], "<svB>");
                if (split.Length > 0 && split[0].Length > 0)
                    sa.Add(split);
            }

            for (int i = 0; i < sa.Count; i++) {
                string category = sa[i][0];
                if (string.IsNullOrEmpty(category))
                    continue;

                switch (category) {
                    #region SAV STATE NUMBER
                    case "SAV STATE NUMBER":
                        Console.WriteLine($"save state number: {sa[i][1]}");
                        break;
                    #endregion
                    #region DENPOS
                    case "DENPOS":
                        Console.WriteLine($"den pos:{sa[i][1]}");
                        break;
                    #endregion
                    #region CYCLENUM
                    case "CYCLENUM":
                        Console.WriteLine($"cycle number:{int.Parse(sa[i][1])}");
                        break;
                    #endregion
                    #region FOOD
                    case "FOOD":
                        Console.WriteLine($"food: {int.Parse(sa[i][1])}");
                        break;
                    #endregion
                    #region NEXTID
                    case "NEXTID":
                        Console.WriteLine($"next id: {int.Parse(sa[i][1])}");
                        break;
                    #endregion
                    #region HASTHEGLOW
                    case "HASTHEGLOW":
                        Console.WriteLine("has the glow");
                        break;
                    #endregion
                    #region GUIDEOVERSEERDEAD
                    case "GUIDEOVERSEERDEAD":
                        Console.WriteLine("guide overseer dead");
                        break;
                    #endregion
                    #region RESPAWNS
                    case "RESPAWNS":
                        Console.WriteLine("respawns:");

                        string[] respawnArray = sa[i][1].Split('.');
                        foreach (string r in respawnArray)
                            Console.WriteLine($"  respawn: {r}");
                        break;
                    #endregion
                    #region WAITRESPAWNS
                    case "WAITRESPAWNS":
                        Console.WriteLine("wait respawns:");

                        string[] waitRespawnArray = sa[i][1].Split('.');
                        foreach (string r in waitRespawnArray)
                            Console.WriteLine($"respawn: {r}");
                        break;
                    #endregion
                    #region REGIONSTATE
                    case "REGIONSTATE":
                        Console.WriteLine("region: ");

                        string[] regionStateSplit = Regex.Split(sa[i][1], "<rgA>");
                        foreach (string region in regionStateSplit) {
                            string[] rd = Regex.Split(region, "<rgB>");
                            if (rd[0] == "REGIONNAME") {
                                Console.WriteLine($"  {rd[1]}:");

                                string[] rdd = Regex.Split(sa[i][1], "<rgA>");
                                foreach (string r in rdd) {
                                    string[] rddd = Regex.Split(r, "<rgA>");
                                    foreach (string s in rddd) {
                                        string[] rdata = Regex.Split(s, "<rgB>");

                                        switch (rdata[0]) {
                                            #region LASTCYCLEUPDATED
                                            case "LASTCYCLEUPDATED":
                                                Console.WriteLine($"  last cycle updated: {int.Parse(rdata[1])}");
                                                break;
                                            #endregion
                                            #region LINEAGES
                                            case "LINEAGES":
                                                Console.WriteLine("  lineages:");

                                                string[] rdlcs = rdata[1].Split('.');
                                                foreach (string rdlc in rdlcs)
                                                    Console.WriteLine($"    {int.Parse(rdlc)}");
                                                break;
                                            #endregion
                                            #region OBJECTS
                                            case "OBJECTS":
                                                Console.WriteLine("  objects:");

                                                string[] objs = Regex.Split(rdata[1], "<rgC>");
                                                foreach (string obj in objs) {
                                                    if (string.IsNullOrEmpty(obj))
                                                        continue;
                                                    string[] objd = Regex.Split(obj, "<oA>");

                                                    Console.WriteLine($"    {objd[1]}"); // type
                                                    Console.WriteLine($"      id:{objd[0]}"); // id
                                                    string[] objpos = objd[2].Split('.'); // position
                                                    Console.WriteLine($"      pos: {objpos[0]},{objpos[1]} {objpos[2]} {objpos[3]}");

                                                    for (int misc = 3; misc < objd.Length; misc++)
                                                        Console.WriteLine($"      extra save data {misc - 3}: {objd[misc]}");
                                                }
                                                break;
                                            #endregion
                                            #region POPULATION
                                            case "POPULATION":
                                                Console.WriteLine("  population:");

                                                string[] pops = Regex.Split(rdata[1], "<rgC>");
                                                foreach (string pop in pops) {
                                                    if (string.IsNullOrEmpty(pop))
                                                        continue;

                                                    Console.WriteLine("    creature:");
                                                    string[] critData = Regex.Split(pop, "<cA>");

                                                    string[] id = critData[1].Split('.');
                                                    Console.WriteLine($"      spawner: {id[1]} number: {id[2]}");

                                                    string[] den = critData[2].Split('.');
                                                    Console.WriteLine($"      room: {den[0]} abstract node: {den[1]}");

                                                    Console.WriteLine("      state:");
                                                    string[] cs = Regex.Split(critData[3], "<cB>");
                                                    foreach (string sv in cs) {
                                                        string[] svd = Regex.Split(sv, "<cC>");

                                                        switch (svd[0]) {
                                                            #region Dead
                                                            case "Dead":
                                                                Console.WriteLine("        dead");
                                                                break;
                                                            #endregion
                                                            #region Social
                                                            case "Social":
                                                                Console.WriteLine("        social memory:");

                                                                string[] sm = Regex.Split(svd[1], "<smA>");
                                                                foreach (string smv in sm) {
                                                                    string[] rel = Regex.Split(smv, "<rA>");

                                                                    if (rel[0] == "REL") {
                                                                        Console.WriteLine("          relationship:");
                                                                        Console.WriteLine($"            towards: {rel[1]}");

                                                                        foreach (string relationship in rel) {

                                                                            string[] relationshipData = Regex.Split(relationship, "<rB>");
                                                                            switch (relationshipData[0]) {
                                                                                #region L
                                                                                case "L":
                                                                                    Console.WriteLine($"              like: {relationshipData[1]}");
                                                                                    break;
                                                                                #endregion
                                                                                #region F
                                                                                case "F":
                                                                                    Console.WriteLine($"              fear: {relationshipData[1]}");
                                                                                    break;
                                                                                #endregion
                                                                                #region K
                                                                                case "K":
                                                                                    Console.WriteLine($"              kill: {relationshipData[1]}");
                                                                                    break;
                                                                                    #endregion
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                                break;
                                                            #endregion
                                                            #region SpawnData
                                                            case "SpawnData":
                                                                Console.WriteLine($"        spawn data: {svd[1]}");
                                                                break;
                                                            #endregion
                                                            #region MeatLeft
                                                            case "MeatLeft":
                                                                Console.WriteLine($"        meat left: {int.Parse(svd[1])}");
                                                                break;
                                                                #endregion 
                                                        }
                                                    }
                                                }
                                                break;
                                            #endregion
                                                // not editable in editor yet
                                            #region SWARMROOMS
                                            case "SWARMROOMS":
                                                Console.WriteLine("  swarm rooms:");

                                                string[] rdsws = rdata[1].Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                                                foreach (string rdsw in rdsws)
                                                    Console.WriteLine($"    {int.Parse(rdsw)}");
                                                break;
                                            #endregion
                                            #region STICKS
                                            case "STICKS":
                                                Console.WriteLine("  sticks:");

                                                string[] sticks = Regex.Split(rdata[1], "<rgC>");
                                                foreach (string stick in sticks)
                                                    Console.WriteLine($"    {stick}");
                                                break;
                                            #endregion
                                            #region CONSUMEDITEMS
                                            case "CONSUMEDITEMS":
                                                Console.WriteLine("  consumed items:");

                                                string[] cons = Regex.Split(rdata[1], "<rgC>");
                                                foreach (string con in cons)
                                                    Console.WriteLine($"    {con}");
                                                break;
                                            #endregion
                                            #region ROOMSVISITED
                                            case "ROOMSVISITED":
                                                Console.WriteLine("  rooms visited:");

                                                foreach (char rv in rdata[1])
                                                    Console.WriteLine($"    {rv == '1'}");
                                                break;
                                            #endregion

                                            default: break;
                                        }
                                    }
                                }
                                // CREATURE STUFF
                                //Console.WriteLine($"  load string: {sa[i][1]}");
                                break;
                            }
                        }
                        break;
                    #endregion
                    #region COMMUNITIES
                    case "COMMUNITIES":
                        // RELATIONSHIP STUFF
                        //Console.WriteLine($"communities: {sa[i][1]}");
                        Console.WriteLine("communities:");

                        string[] coms = Regex.Split(sa[i][1], "<ccA>");
                        foreach(string com in coms) {
                            if (string.IsNullOrEmpty(com))
                                continue;

                            //string[] comData = Regex.Split(com, "<ccB>");
                            //switch (comData[0]) {
                            //    case "SCAVSHY":
                            //        Console.WriteLine("  scavengers:");
                            //        Console.WriteLine($"    {comData[1]}");
                            //        break;
                            //}
                            Console.WriteLine($"    {com}");
                        }

                        break;
                    #endregion
                    #region MISCWORLDSAVEDATA
                    case "MISCWORLDSAVEDATA":
                        Console.WriteLine("misc world save data:");

                        string[] miscWorldSplit = Regex.Split(sa[i][1], "<mwA>");
                        foreach (string mw in miscWorldSplit) {
                            string[] data = Regex.Split(mw, "<mwB>");
                            switch (data[0]) {
                                // pebbles
                                #region SSaiConversationsHad
                                case "SSaiConversationsHad":
                                    Console.WriteLine($"  pebbles conversations: {int.Parse(data[1])}");
                                    break;
                                #endregion
                                #region SSaiThrowOuts
                                case "SSaiThrowOuts":
                                    Console.WriteLine($"  pebbles throwouts: {int.Parse(data[1])}");
                                    break;
                                #endregion
                                // moon
                                #region SLaiState
                                case "SLaiState":
                                    Console.WriteLine("  moon:");

                                    string[] mn = Regex.Split(data[1], "<slosA>");
                                    foreach (string m in mn) {
                                        string[] mdata = Regex.Split(m, "<slosB>");
                                        switch (mdata[0]) {
                                            #region integersArray
                                            case "integersArray":
                                                Console.WriteLine("    integers:");

                                                string[] mints = mdata[1].Split('.');
                                                foreach (string mint in mints)
                                                    Console.WriteLine($"      {int.Parse(mint)}");
                                                break;
                                            #endregion
                                            #region miscBools
                                            case "miscBools":
                                                Console.WriteLine("    bools:");

                                                foreach (char mbool in mdata[1])
                                                    Console.WriteLine($"      {mbool == '1'}");
                                                break;
                                            #endregion
                                            #region significantPearls
                                            case "significantPearls":
                                                Console.WriteLine("    significant pearls:");

                                                foreach (char msp in mdata[1])
                                                    Console.WriteLine($"      {msp == '1'}");
                                                break;
                                            #endregion
                                            #region miscItemsDescribed
                                            case "miscItemsDescribed":
                                                Console.WriteLine("    misc items described:");

                                                foreach (char mid in mdata[1])
                                                    Console.WriteLine($"      {mid == '1'}");
                                                break;
                                            #endregion
                                            #region likesPlayer
                                            case "likesPlayer":
                                                Console.WriteLine($"    player like: {float.Parse(mdata[1])}");
                                                break;
                                            #endregion
                                            #region itemsAlreadyTalkedAbout
                                            case "itemsAlreadyTalkedAbout":
                                                Console.WriteLine("    items already described:");

                                                string[] iat = Regex.Split(mdata[1], "<slosC>");
                                                foreach (string s in iat) {
                                                    Console.WriteLine("      entity:");

                                                    string[] eid = s.Split('.');
                                                    Console.WriteLine($"        spawner: {eid[1]}");
                                                    Console.WriteLine($"        number: {eid[2]}");
                                                }
                                                break;
                                            #endregion

                                            default: break;
                                        }
                                    }
                                    break;
                                #endregion
                                #region playerGuideState
                                case "playerGuideState":
                                    Console.WriteLine("  player guide state:");

                                    string[] pgd = Regex.Split(data[1], "<pgsA>");
                                    foreach (string pg in pgd) {
                                        string[] pgdata = Regex.Split(pg, "<pgsB>");

                                        switch (pgdata[0]) {
                                            #region integersArray
                                            case "integersArray":
                                                Console.WriteLine("    integers:");

                                                string[] pgints = pgdata[1].Split('.');
                                                foreach (string pgint in pgints)
                                                    Console.WriteLine($"      {int.Parse(pgint)}");
                                                break;
                                            #endregion
                                            #region itemTypes
                                            case "itemTypes":
                                                Console.WriteLine("    item types:");

                                                foreach (char pgit in pgdata[1])
                                                    Console.WriteLine($"      {pgit == '1'}");
                                                break;
                                            #endregion
                                            #region creatureTypes
                                            case "creatureTypes":
                                                Console.WriteLine("    creature types:");

                                                foreach (char pgct in pgdata[1])
                                                    Console.WriteLine($"      {pgct == '1'}");
                                                break;
                                            #endregion
                                            #region likesPlayer
                                            case "likesPlayer":
                                                Console.WriteLine($"    player like: {float.Parse(pgdata[1])}");
                                                break;
                                            #endregion
                                            #region directionHandHolding
                                            case "directionHandHolding":
                                                Console.WriteLine($"    direction handholding: {float.Parse(pgdata[1])}");
                                                break;
                                            #endregion
                                            #region imagesShown
                                            case "imagesShown":
                                                Console.WriteLine("    images shown:");

                                                string[] pgimgs = pgdata[1].Split('.');
                                                foreach (string pgimg in pgimgs)
                                                    Console.WriteLine($"      {pgimg}");
                                                break;
                                            #endregion
                                            #region forcedDirsGiven
                                            case "forcedDirsGiven":
                                                Console.WriteLine("    forced directions:");

                                                string[] pgforcedDirs = pgdata[1].Split('.');
                                                foreach (string pgforcedDir in pgforcedDirs) {
                                                    string[] pgkvps = pgforcedDir.Split(',');
                                                    Console.WriteLine($"      {pgkvps[0]},{pgkvps[1]}");
                                                }
                                                break;
                                            #endregion

                                            default: break;
                                        }
                                    }
                                    break;
                                #endregion
                                #region MOONREVIVED
                                case "MOONREVIVED":
                                    Console.WriteLine("  moon revived");
                                    break;
                                #endregion
                                #region PEBBLESHELPED
                                case "PEBBLESHELPED":
                                    Console.WriteLine("  pebbles has seen the green neuron");
                                    break;
                                #endregion
                                #region MEMORYFROLICK
                                case "MEMORYFROLICK":
                                    Console.WriteLine("  slugcat has frolicked in memory arrays");
                                    break;
                                #endregion

                                default: break;
                            }
                        }
                        break;
                    #endregion
                    #region DEATHPERSISTENTSAVEDATA
                    case "DEATHPERSISTENTSAVEDATA":
                        Console.WriteLine("death persistent save data:");

                        string[] dpdatas = Regex.Split(sa[i][1], "<dpA>");
                        foreach (string dpd in dpdatas) {
                            string[] dpdata = Regex.Split(dpd, "<dpB>");
                            switch (dpdata[0]) {
                                #region KARMA
                                case "KARMA":
                                    Console.WriteLine($"  karma: {int.Parse(dpdata[1])}");
                                    break;
                                #endregion
                                #region KARMACAP
                                case "KARMACAP":
                                    Console.WriteLine($"  karma cap: {int.Parse(dpdata[1])}");
                                    break;
                                #endregion
                                #region REINFORCEDKARMA
                                case "REINFORCEDKARMA":
                                    Console.WriteLine($"  has karma flower: {dpdata[0] == "1"}");
                                    break;
                                #endregion
                                #region FLOWERPOS
                                case "FLOWERPOS":
                                    Console.WriteLine("  karma flower position:");

                                    string[] dpkfp = dpdata[1].Split('.');
                                    Console.WriteLine($"    {dpkfp[0]}.{dpkfp[1]}.{dpkfp[2]}.{dpkfp[3]}");
                                    break;
                                #endregion
                                #region GHOSTS
                                case "GHOSTS":
                                    Console.WriteLine("  echoes talked to:");

                                    string[] dpetts = dpdata[1].Split('.');
                                    foreach (string dpett in dpetts)
                                        Console.WriteLine($"    {int.Parse(dpett)}");
                                    break;
                                #endregion
                                #region SONGSPLAYRECORDS
                                case "SONGSPLAYRECORDS":
                                    Console.WriteLine("  songs played:");

                                    string[] dpSongsPlayed = Regex.Split(dpdata[1], "<dpC>");
                                    foreach (string dpsong in dpSongsPlayed) {
                                        string[] dpSongData = Regex.Split(dpsong, "<dpD>");
                                        Console.WriteLine($"    {dpSongData[0]} {dpSongData[1]}");
                                    }
                                    break;
                                #endregion
                                #region SESSIONRECORDS
                                case "SESSIONRECORDS":
                                    Console.WriteLine("  session records:");

                                    string[] dpSessionRecords = Regex.Split(dpdata[1], "<dpC>");
                                    foreach (string dpSessionRecord in dpSessionRecords) {
                                        Console.WriteLine("    record:");
                                        Console.WriteLine($"      survived: {dpSessionRecord[0] == '1'}");
                                        Console.WriteLine($"      travelled: {dpSessionRecord[1] == '1'}");
                                    }
                                    break;
                                #endregion
                                #region WINSTATE
                                case "WINSTATE":
                                    Console.WriteLine("  win state:");

                                    string[] dpws = Regex.Split(dpdata[1], "<wsA>");
                                    foreach (string dpwsdata in dpws) {
                                        string[] dpwseg = Regex.Split(dpwsdata, "<egA>");
                                        Console.WriteLine($"    {dpwseg[0]}");
                                    }
                                    break;
                                #endregion
                                #region CONSUMEDFLOWERS
                                case "CONSUMEDFLOWERS":
                                    Console.WriteLine("  consumed flowers:");

                                    string[] dpcf = Regex.Split(dpdata[1], "<dpC>");
                                    foreach (string dpcfdata in dpcf) {
                                        Console.WriteLine("    flower:");

                                        string[] dpcfflowerdata = dpcfdata.Split('.');
                                        Console.WriteLine($"      origin room: {dpcfflowerdata[0]}");
                                        Console.WriteLine($"      placed object index: {dpcfflowerdata[1]}");
                                        Console.WriteLine($"      wait cycles: {dpcfflowerdata[2]}");
                                    }
                                    break;
                                #endregion
                                #region HASTHEMARK
                                case "HASTHEMARK":
                                    Console.WriteLine("  slugcat has mark of communication");
                                    break;
                                #endregion
                                #region TUTMESSAGES
                                case "TUTMESSAGES":
                                    Console.WriteLine("  tutorial messages:");

                                    foreach (char dptutmsg in dpdata[1])
                                        Console.WriteLine($"    {dptutmsg == '1'}");
                                    break;
                                #endregion
                                #region METERSSHOWN
                                case "METERSSHOWN":
                                    Console.WriteLine("  meters shown:");

                                    foreach (char dpmeter in dpdata[1])
                                        Console.WriteLine($"    {dpmeter == '1'}");
                                    break;
                                #endregion
                                #region FOODREPBONUS
                                case "FOODREPBONUS":
                                    Console.WriteLine($"  food replenish bonus: {int.Parse(dpdata[1])}");
                                    break;
                                #endregion
                                #region DDWORLDVERSION
                                case "DDWORLDVERSION":
                                    Console.WriteLine($"  world version: {int.Parse(dpdata[1])}");
                                    break;
                                #endregion
                                #region DEATHS
                                case "DEATHS":
                                    Console.WriteLine($"    deaths: {int.Parse(dpdata[1])}");
                                    break;
                                #endregion
                                #region SURVIVES
                                case "SURVIVES":
                                    Console.WriteLine($"  survives: {int.Parse(dpdata[1])}");
                                    break;
                                #endregion
                                #region QUITS
                                case "QUITS":
                                    Console.WriteLine($"  quits: {int.Parse(dpdata[1])}");
                                    break;
                                #endregion
                                #region DEATHPOSS
                                case "DEATHPOSS":
                                    Console.WriteLine("  death positions:");

                                    string[] dpdeathposs = Regex.Split(dpdata[1], "<dpC>");
                                    foreach (string dpdeathdata in dpdeathposs) {
                                        string[] dpdeath = dpdeathdata.Split('.');

                                        // x, y, room, abstract node
                                        Console.WriteLine($"    {dpdeath[0]},{dpdeath[1]} {dpdeath[2]} {dpdeath[3]}");
                                    }
                                    break;
                                #endregion
                                #region REDSDEATH
                                case "REDSDEATH":
                                    Console.WriteLine("  hunter is dead");
                                    break;
                                #endregion
                                #region ASCENDED
                                case "ASCENDED":
                                    Console.WriteLine("  slugcat has ascended");
                                    break;
                                #endregion
                                #region PHIRKC
                                case "PHIRKC":
                                    Console.WriteLine("  pebbles has increased hunter's karma cap");
                                    break;
                                #endregion
                                #region UNLOCKEDGATES
                                case "UNLOCKEDGATES":
                                    Console.WriteLine("  unlocked gates:");

                                    string[] dpug = Regex.Split(dpdata[1], "<dpC>");
                                    foreach (string dpugdata in dpug) {
                                        Console.WriteLine($"    {dpugdata}");
                                    }
                                    break;
                                #endregion

                                default: break;
                            }
                        }
                        break;
                    #endregion
                    #region SWALLOWEDITEMS
                    case "SWALLOWEDITEMS":
                        Console.WriteLine("swallowed items:");

                        for (int swi = 1; swi < sa[i].Length; swi++) {
                            Console.WriteLine($"    {sa[i][swi]}");
                        }
                        break;
                    #endregion
                    #region VERSION
                    case "VERSION":
                        Console.WriteLine($"game version: {int.Parse(sa[i][1])}");
                        break;
                    #endregion
                    #region INITVERSION
                    case "INITVERSION":
                        Console.WriteLine($"initial game version: {int.Parse(sa[i][1])}");
                        break;
                    #endregion
                    #region WORLDVERSION
                    case "WORLDVERSION":
                        Console.WriteLine($"world version: {int.Parse(sa[i][1])}");
                        break;
                    #endregion
                    #region SEED
                    case "SEED":
                        Console.WriteLine($"seed: {int.Parse(sa[i][1])}");
                        break;
                    #endregion
                    #region DREAMSSTATE
                    case "DREAMSSTATE":
                        Console.WriteLine("dreams state:");

                        string[] ds = Regex.Split(sa[i][1], "<dsA>");
                        foreach (string dsd in ds) {
                            string[] dsdata = Regex.Split(dsd, "<dsB>");

                            switch (dsdata[0]) {
                                #region integerArray
                                case "integerArray":
                                    Console.WriteLine("  integer array:");

                                    string[] dsints = dsdata[1].Split('.');
                                    foreach (string dsint in dsints)
                                        Console.WriteLine($"    {int.Parse(dsint)}");
                                    break;
                                #endregion
                                default: break;
                            }
                        }
                        break;
                    #endregion
                    #region TOTFOOD
                    case "TOTFOOD":
                        Console.WriteLine($"total food: {int.Parse(sa[i][1])}");
                        break;
                    #endregion
                    #region TOTTIME
                    case "TOTTIME":
                        Console.WriteLine($"total time: {int.Parse(sa[i][1])}");
                        break;
                    #endregion
                    #region CURRVERCYCLES
                    case "CURRVERCYCLES":
                        Console.WriteLine($"cycles in current rainworld version: {int.Parse(sa[i][1])}");
                        break;
                    #endregion
                    #region KILLS
                    case "KILLS":
                        Console.WriteLine("kills:");

                        string[] k = Regex.Split(sa[i][1], "<svC>");
                        foreach (string kill in k) {
                            string[] killdata = Regex.Split(kill, "<svD>");

                            Console.WriteLine($"  {killdata[0]} {killdata[1]}");
                        }
                        break;
                    #endregion
                    #region REDEXTRACYCLES
                    case "REDEXTRACYCLES":
                        Console.WriteLine("hunter has extra cycles");
                        break;
                    #endregion

                    default: break;
                }
            }

            Console.ReadLine();
        }
    }

    class Save {
        public int SaveStateNumber { get; set; }
        public string DenPosition { get; set; }
        public int CycleNumber { get; set; }
        public int Food { get; set; }
        public int NextID { get; set; }
        public bool HasTheGlow { get; set; }
        public bool GuideOverseerDead { get; set; }
        public List<int> Respawns { get; set; } = new List<int>();
        public List<int> WaitRespawns { get; set; } = new List<int>();
        public List<RegionState> RegionStates { get; set; } = new List<RegionState>();
        public List<Community> Communities { get; set; } = new List<Community>();

        public class ID {
            public int Spawner { get; set; }
            public int Number { get; set; }

            public ID(int spawner, int number) {
                Spawner = spawner;
                Number = number;
            }
        }

        public class Position {
            public int X { get; set; }
            public int Y { get; set; }
            public int Room { get; set; }
            public int AbstractNode { get; set; }

            public Position(int x, int y, int room, int abstractNode) {
                X = x;
                Y = y;
                Room = room;
                AbstractNode = abstractNode;
            }
        }

        public class Object {
            public string Type { get; set; }
            public ID ID { get; set; }
            public Position Position { get; set; }
            public List<string> MiscellaneousData { get; set; } = new List<string>();

            public Object(string type, ID id, Position position, params string[] misc) {
                Type = type;
                ID = id;
                Position = position;
                MiscellaneousData = misc.ToList();
            }
        }

        public class Creature {
            public ID ID { get; set; }
            public int Room { get; set; }
            public int AbstractRoom { get; set; }
            public bool Dead { get; set; }
            public Dictionary<ID, (float like, float fear, float kill)> Relationships { get; set; } = new Dictionary<ID, (float like, float fear, float kill)>();
            public string SpawnData { get; set; }
            public int MeatLeft { get; set; }

            public Creature(ID id, int room, int abstractRoom, bool dead, string spawnData, int meatLeft, params (ID id, float like, float fear, float kill)[] relationships) {
                ID = id;
                Room = room;
                AbstractRoom = abstractRoom;
                Dead = dead;
                SpawnData = spawnData;
                MeatLeft = meatLeft;

                foreach (var r in relationships)
                    Relationships[r.id] = (r.like, r.fear, r.kill);
            }
        }

        public class RegionState {
            public string Name { get; set; }
            public int LastCycleUpdated { get; set; }
            public List<int> SwarmRooms { get; set; } = new List<int>();
            public List<int> Lineages { get; set; } = new List<int>();
            public List<Object> Objects { get; set; } = new List<Object>();
            public List<Creature> Population { get; set; } = new List<Creature>();

            public RegionState(string name, int lastCycleUpdated) {
                Name = name;
                LastCycleUpdated = lastCycleUpdated;
            }
        }

        public class Community {
            public string Data { get; set; }
        }

        public class MiscWorldData {
            public int ConversationsWithPebbles { get; set; }
            public int PebblesThrownOutCount { get; set; }

        }
    }
}
