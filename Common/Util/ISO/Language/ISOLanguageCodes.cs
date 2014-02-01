﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Frost.Common.Util.ISO {

    /// <summary>Represents a mapping between english language names and their officialy assigned ISO 639 language codes.</summary>
    public sealed class ISOLanguageCodes : ISOCodes<ISOLanguageCode> {

        private static readonly ISOLanguageCodes _instance = new ISOLanguageCodes();
        private static readonly Dictionary<string, string> LanguageNames = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        static ISOLanguageCodes() {
            ISOLanguageCode[] ilc = {
                #region Language Codes
                new ISOLanguageCode("Afar", "aa", "aar"),
                new ISOLanguageCode("Abkhazian", "ab", "abk"),
                new ISOLanguageCode("Achinese", null, "ace"),
                new ISOLanguageCode("Acoli", null, "ach"),
                new ISOLanguageCode("Adangme", null, "ada"),
                new ISOLanguageCode(null, "ady", null, "Adyghe", "Adygei"),
                new ISOLanguageCode("Afro-Asiatic languages", null, "afa"),
                new ISOLanguageCode("Afrihili", null, "afh"),
                new ISOLanguageCode("Afrikaans", "af", "afr"),
                new ISOLanguageCode("Ainu", null, "ain"),
                new ISOLanguageCode("Akan", "ak", "aka"),
                new ISOLanguageCode("Akkadian", null, "akk"),
                new ISOLanguageCode("Albanian", "sq", "alb", "sqi"),
                new ISOLanguageCode("Aleut", null, "ale"),
                new ISOLanguageCode("Algonquian languages", null, "alg"),
                new ISOLanguageCode("Southern Altai", null, "alt"),
                new ISOLanguageCode("Amharic", "am", "amh"),
                new ISOLanguageCode("English, Old (ca.450-1100)", null, "ang"),
                new ISOLanguageCode("Angika", null, "anp"),
                new ISOLanguageCode("Apache languages", null, "apa"),
                new ISOLanguageCode("Arabic", "ar", "ara"),
                new ISOLanguageCode("Official Aramaic (700-300 BCE), Imperial Aramaic (700-300 BCE)", null, "arc"),
                new ISOLanguageCode("Aragonese", "an", "arg"),
                new ISOLanguageCode("Armenian", "hy", "arm", "hye"),
                new ISOLanguageCode(null, "arn", null, "Mapudungun", "Mapuche"),
                new ISOLanguageCode("Arapaho", null, "arp"),
                new ISOLanguageCode("Artificial languages", null, "art"),
                new ISOLanguageCode("Arawak", null, "arw"),
                new ISOLanguageCode("Assamese", "as", "asm"),
                new ISOLanguageCode(null, "ast", null, "Asturian", "Bable", "Leonese", "Asturleonese"),
                new ISOLanguageCode("Athapascan languages", null, "ath"),
                new ISOLanguageCode("Australian languages", null, "aus"),
                new ISOLanguageCode("Avaric", "av", "ava"),
                new ISOLanguageCode("Avestan", "ae", "ave"),
                new ISOLanguageCode("Awadhi", null, "awa"),
                new ISOLanguageCode("Aymara", "ay", "aym"),
                new ISOLanguageCode("Azerbaijani", "az", "aze"),
                new ISOLanguageCode("Banda languages", null, "bad"),
                new ISOLanguageCode("Bamileke languages", null, "bai"),
                new ISOLanguageCode("Bashkir", "ba", "bak"),
                new ISOLanguageCode("Baluchi", null, "bal"),
                new ISOLanguageCode("Bambara", "bm", "bam"),
                new ISOLanguageCode("Balinese", null, "ban"),
                new ISOLanguageCode("Basque", "eu", "baq", "eus"),
                new ISOLanguageCode("Basa", null, "bas"),
                new ISOLanguageCode("Baltic languages", null, "bat"),
                new ISOLanguageCode(null, "bej", null, "Beja", "Bedawiyet"),
                new ISOLanguageCode("Belarusian", "be", "bel"),
                new ISOLanguageCode("Bemba", null, "bem"),
                new ISOLanguageCode("Bengali", "bn", "ben"),
                new ISOLanguageCode("Berber languages", null, "ber"),
                new ISOLanguageCode("Bhojpuri", null, "bho"),
                new ISOLanguageCode("Bihari languages", "bh", "bih"),
                new ISOLanguageCode("Bikol", null, "bik"),
                new ISOLanguageCode("Bini, Edo", null, "bin"),
                new ISOLanguageCode("Bislama", "bi", "bis"),
                new ISOLanguageCode("Siksika", null, "bla"),
                new ISOLanguageCode("Bantu languages", null, "bnt"),
                new ISOLanguageCode("Bosnian", "bs", "bos"),
                new ISOLanguageCode("Braj", null, "bra"),
                new ISOLanguageCode("Breton", "br", "bre"),
                new ISOLanguageCode("Batak languages", null, "btk"),
                new ISOLanguageCode("Buriat", null, "bua"),
                new ISOLanguageCode("Buginese", null, "bug"),
                new ISOLanguageCode("Bulgarian", "bg", "bul"),
                new ISOLanguageCode("Burmese", "my", "bur", "mya"),
                new ISOLanguageCode("Blin, Bilin", null, "byn"),
                new ISOLanguageCode("Caddo", null, "cad"),
                new ISOLanguageCode("Central American Indian languages", null, "cai"),
                new ISOLanguageCode("Galibi Carib", null, "car"),
                new ISOLanguageCode("Catalan, Valencian", "ca", "cat"),
                new ISOLanguageCode("Caucasian languages", null, "cau"),
                new ISOLanguageCode("Cebuano", null, "ceb"),
                new ISOLanguageCode("Celtic languages", null, "cel"),
                new ISOLanguageCode("Czech", "cs", "cze", "ces"),
                new ISOLanguageCode("Chamorro", "ch", "cha"),
                new ISOLanguageCode("Chibcha", null, "chb"),
                new ISOLanguageCode("Chechen", "ce", "che"),
                new ISOLanguageCode("Chagatai", null, "chg"),
                new ISOLanguageCode("Chinese", "zh", "chi", "zho"),
                new ISOLanguageCode("Chuukese", null, "chk"),
                new ISOLanguageCode("Mari", null, "chm"),
                new ISOLanguageCode("Chinook jargon", null, "chn"),
                new ISOLanguageCode("Choctaw", null, "cho"),
                new ISOLanguageCode(null, "chp", null, "Chipewyan", "Dene Suline"),
                new ISOLanguageCode("Cherokee", null, "chr"),
                new ISOLanguageCode("cu", "chu", null, "Church Slavic", "Old Slavonic", "Church Slavonic", "Old Bulgarian", "Old Church Slavonic"),
                new ISOLanguageCode("Chuvash", "cv", "chv"),
                new ISOLanguageCode("Cheyenne", null, "chy"),
                new ISOLanguageCode("Chamic languages", null, "cmc"),
                new ISOLanguageCode("Coptic", null, "cop"),
                new ISOLanguageCode("Cornish", "kw", "cor"),
                new ISOLanguageCode("Corsican", "co", "cos"),
                //new ISOLanguageCode("Creoles and pidgins, English based", null, "cpe"),
                //new ISOLanguageCode("Creoles and pidgins, French-based", null, "cpf"),
                //new ISOLanguageCode("Creoles and pidgins, Portuguese-based", null, "cpp"),
                new ISOLanguageCode("Cree", "cr", "cre"),
                new ISOLanguageCode(null, "crh", null, "Crimean Tatar", "Crimean Turkish"),
                new ISOLanguageCode("Creoles and pidgins", null, "crp"),
                new ISOLanguageCode("Kashubian", null, "csb"),
                new ISOLanguageCode("Cushitic languages", null, "cus"),
                new ISOLanguageCode("Dakota", null, "dak"),
                new ISOLanguageCode("da", "dan", null, "Danish", "Dansk"),
                new ISOLanguageCode("Dargwa", null, "dar"),
                new ISOLanguageCode("Land Dayak languages", null, "day"),
                new ISOLanguageCode("Delaware", null, "del"),
                new ISOLanguageCode("Slave (Athapascan)", null, "den"),
                new ISOLanguageCode("Dogrib", null, "dgr"),
                new ISOLanguageCode("Dinka", null, "din"),
                new ISOLanguageCode("dv", "div", null, "Divehi", "Dhivehi", "Maldivian"),
                new ISOLanguageCode("Dogri", null, "doi"),
                new ISOLanguageCode("Dravidian languages", null, "dra"),
                new ISOLanguageCode("Lower Sorbian", null, "dsb"),
                new ISOLanguageCode("Duala", null, "dua"),
                new ISOLanguageCode("Dutch, Middle (ca.1050-1350)", null, "dum"),
                new ISOLanguageCode("nl", "dut", "nld", "Dutch", "Flemish"),
                new ISOLanguageCode("Dyula", null, "dyu"),
                new ISOLanguageCode("Dzongkha", "dz", "dzo"),
                new ISOLanguageCode("Efik", null, "efi"),
                new ISOLanguageCode("Egyptian (Ancient)", null, "egy"),
                new ISOLanguageCode("Ekajuk", null, "eka"),
                new ISOLanguageCode("Elamite", null, "elx"),
                new ISOLanguageCode("English", "en", "eng"),
                new ISOLanguageCode("English, Middle (1100-1500)", null, "enm"),
                new ISOLanguageCode("Esperanto", "eo", "epo"),
                new ISOLanguageCode("Estonian", "et", "est"),
                new ISOLanguageCode("Ewe", "ee", "ewe"),
                new ISOLanguageCode("Ewondo", null, "ewo"),
                new ISOLanguageCode("Fang", null, "fan"),
                new ISOLanguageCode("Faroese", "fo", "fao"),
                new ISOLanguageCode("Fanti", null, "fat"),
                new ISOLanguageCode("Fijian", "fj", "fij"),
                new ISOLanguageCode("Filipino, Pilipino", null, "fil"),
                new ISOLanguageCode("fi", "fin", null, "Finnish", "Suomi"),
                new ISOLanguageCode("Finno-Ugrian languages", null, "fiu"),
                new ISOLanguageCode("Fon", null, "fon"),
                new ISOLanguageCode("fr", "fre", "fra", "French", "Français"),
                new ISOLanguageCode("French, Middle (ca.1400-1600)", null, "frm"),
                new ISOLanguageCode("French, Old (842-ca.1400)", null, "fro"),
                new ISOLanguageCode("Northern Frisian", null, "frr"),
                new ISOLanguageCode("Eastern Frisian", null, "frs"),
                new ISOLanguageCode("Western Frisian", "fy", "fry"),
                new ISOLanguageCode("Fulah", "ff", "ful"),
                new ISOLanguageCode("Friulian", null, "fur"),
                new ISOLanguageCode("Ga", null, "gaa"),
                new ISOLanguageCode("Gayo", null, "gay"),
                new ISOLanguageCode("Gbaya", null, "gba"),
                new ISOLanguageCode("Germanic languages", null, "gem"),
                new ISOLanguageCode("Georgian", "ka", "geo", "kat"),
                new ISOLanguageCode("de", "ger", "deu", "German", "Deutsch"),
                new ISOLanguageCode("Geez", null, "gez"),
                new ISOLanguageCode("Gilbertese", null, "gil"),
                new ISOLanguageCode("gd", "gla", null, "Gaelic", "Scottish Gaelic"),
                new ISOLanguageCode("Irish", "ga", "gle"),
                new ISOLanguageCode("Galician", "gl", "glg"),
                new ISOLanguageCode("Manx", "gv", "glv"),
                new ISOLanguageCode("German, Middle High (ca.1050-1500)", null, "gmh"),
                new ISOLanguageCode("German, Old High (ca.750-1050)", null, "goh"),
                new ISOLanguageCode("Gondi", null, "gon"),
                new ISOLanguageCode("Gorontalo", null, "gor"),
                new ISOLanguageCode("Gothic", null, "got"),
                new ISOLanguageCode("Grebo", null, "grb"),
                new ISOLanguageCode("Greek, Ancient (to 1453)", null, "grc"),
                new ISOLanguageCode("el", "gre", "ell", "Greek", "Greek, Modern (1453-)"),
                new ISOLanguageCode("Guarani", "gn", "grn"),
                new ISOLanguageCode(null, "gsw", null, "Swiss German", "Alemannic", "Alsatian"),
                new ISOLanguageCode("Gujarati", "gu", "guj"),
                new ISOLanguageCode("Gwich'in", null, "gwi"),
                new ISOLanguageCode("Haida", null, "hai"),
                new ISOLanguageCode("ht", "hat", null, "Haitian", "Haitian Creole"),
                new ISOLanguageCode("Hausa", "ha", "hau"),
                new ISOLanguageCode("Hawaiian", null, "haw"),
                new ISOLanguageCode("Hebrew", "he", "heb"),
                new ISOLanguageCode("Herero", "hz", "her"),
                new ISOLanguageCode("Hiligaynon", null, "hil"),
                new ISOLanguageCode(null, "him", null, "Himachali languages", "Western Pahari languages"),
                new ISOLanguageCode("Hindi", "hi", "hin"),
                new ISOLanguageCode("Hittite", null, "hit"),
                new ISOLanguageCode("Hmong, Mong", null, "hmn"),
                new ISOLanguageCode("Hiri Motu", "ho", "hmo"),
                new ISOLanguageCode("Croatian", "hr", "hrv"),
                new ISOLanguageCode("Upper Sorbian", null, "hsb"),
                new ISOLanguageCode("Hungarian", "hu", "hun"),
                new ISOLanguageCode("Hupa", null, "hup"),
                new ISOLanguageCode("Iban", null, "iba"),
                new ISOLanguageCode("Igbo", "ig", "ibo"),
                new ISOLanguageCode("Icelandic", "is", "ice", "isl"),
                new ISOLanguageCode("Ido", "io", "ido"),
                new ISOLanguageCode("Sichuan Yi, Nuosu", "ii", "iii"),
                new ISOLanguageCode("Ijo languages", null, "ijo"),
                new ISOLanguageCode("Inuktitut", "iu", "iku"),
                new ISOLanguageCode("ie", "ile", null, "Interlingue", "Occidental"),
                new ISOLanguageCode("Iloko", null, "ilo"),
                new ISOLanguageCode("Interlingua (International Auxiliary Language Association)", "ia", "ina"),
                new ISOLanguageCode("Indic languages", null, "inc"),
                new ISOLanguageCode("Indonesian", "id", "ind"),
                new ISOLanguageCode("Indo-European languages", null, "ine"),
                new ISOLanguageCode("Ingush", null, "inh"),
                new ISOLanguageCode("Inupiaq", "ik", "ipk"),
                new ISOLanguageCode("Iranian languages", null, "ira"),
                new ISOLanguageCode("Iroquoian languages", null, "iro"),
                new ISOLanguageCode("Italian", "it", "ita"),
                new ISOLanguageCode("Javanese", "jv", "jav"),
                new ISOLanguageCode("Lojban", null, "jbo"),
                new ISOLanguageCode("Japanese", "ja", "jpn"),
                new ISOLanguageCode("Judeo-Persian", null, "jpr"),
                new ISOLanguageCode("Judeo-Arabic", null, "jrb"),
                new ISOLanguageCode("Kara-Kalpak", null, "kaa"),
                new ISOLanguageCode("Kabyle", null, "kab"),
                new ISOLanguageCode("Kachin, Jingpho", null, "kac"),
                new ISOLanguageCode("kl", "kal", null, "Kalaallisut", "Greenlandic"),
                new ISOLanguageCode("Kamba", null, "kam"),
                new ISOLanguageCode("Kannada", "kn", "kan"),
                new ISOLanguageCode("Karen languages", null, "kar"),
                new ISOLanguageCode("Kashmiri", "ks", "kas"),
                new ISOLanguageCode("Kanuri", "kr", "kau"),
                new ISOLanguageCode("Kawi", null, "kaw"),
                new ISOLanguageCode("Kazakh", "kk", "kaz"),
                new ISOLanguageCode("Kabardian", null, "kbd"),
                new ISOLanguageCode("Khasi", null, "kha"),
                new ISOLanguageCode("Khoisan languages", null, "khi"),
                new ISOLanguageCode("Central Khmer", "km", "khm"),
                new ISOLanguageCode(null, "kho", null, "Khotanese", "Sakan"),
                new ISOLanguageCode("ki", "kik", null, "Kikuyu", "Gikuyu"),
                new ISOLanguageCode("Kinyarwanda", "rw", "kin"),
                new ISOLanguageCode("ky", "kir", null, "Kirghiz", "Kyrgyz"),
                new ISOLanguageCode("Kimbundu", null, "kmb"),
                new ISOLanguageCode("Konkani", null, "kok"),
                new ISOLanguageCode("Komi", "kv", "kom"),
                new ISOLanguageCode("Kongo", "kg", "kon"),
                new ISOLanguageCode("Korean", "ko", "kor"),
                new ISOLanguageCode("Kosraean", null, "kos"),
                new ISOLanguageCode("Kpelle", null, "kpe"),
                new ISOLanguageCode("Karachay-Balkar", null, "krc"),
                new ISOLanguageCode("Karelian", null, "krl"),
                new ISOLanguageCode("Kru languages", null, "kro"),
                new ISOLanguageCode("Kurukh", null, "kru"),
                new ISOLanguageCode("kj", "kua", null, "Kuanyama", "Kwanyama"),
                new ISOLanguageCode("Kumyk", null, "kum"),
                new ISOLanguageCode("Kurdish", "ku", "kur"),
                new ISOLanguageCode("Kutenai", null, "kut"),
                new ISOLanguageCode("Ladino", null, "lad"),
                new ISOLanguageCode("Lahnda", null, "lah"),
                new ISOLanguageCode("Lamba", null, "lam"),
                new ISOLanguageCode("Lao", "lo", "lao"),
                new ISOLanguageCode("Latin", "la", "lat"),
                new ISOLanguageCode("Latvian", "lv", "lav"),
                new ISOLanguageCode("Lezghian", null, "lez"),
                new ISOLanguageCode("li", "lim", null, "Limburgan", "Limburger", "Limburgish"),
                new ISOLanguageCode("Lingala", "ln", "lin"),
                new ISOLanguageCode("Lithuanian", "lt", "lit"),
                new ISOLanguageCode("Mongo", null, "lol"),
                new ISOLanguageCode("Lozi", null, "loz"),
                new ISOLanguageCode("lb", "ltz", null, "Luxembourgish", "Letzeburgesch"), 
                new ISOLanguageCode("Luba-Lulua", null, "lua"),
                new ISOLanguageCode("Luba-Katanga", "lu", "lub"),
                new ISOLanguageCode("Ganda", "lg", "lug"),
                new ISOLanguageCode("Luiseno", null, "lui"),
                new ISOLanguageCode("Lunda", null, "lun"),
                new ISOLanguageCode("Luo (Kenya and Tanzania)", null, "luo"),
                new ISOLanguageCode("Lushai", null, "lus"),
                new ISOLanguageCode("Macedonian", "mk", "mac", "mkd"),
                new ISOLanguageCode("Madurese", null, "mad"),
                new ISOLanguageCode("Magahi", null, "mag"),
                new ISOLanguageCode("Marshallese", "mh", "mah"),
                new ISOLanguageCode("Maithili", null, "mai"),
                new ISOLanguageCode("Makasar", null, "mak"),
                new ISOLanguageCode("Malayalam", "ml", "mal"),
                new ISOLanguageCode("Mandingo", null, "man"),
                new ISOLanguageCode("Maori", "mi", "mao", "mri"),
                new ISOLanguageCode("Austronesian languages", null, "map"),
                new ISOLanguageCode("Marathi", "mr", "mar"),
                new ISOLanguageCode("Masai", null, "mas"),
                new ISOLanguageCode("Malay", "ms", "may", "msa"),
                new ISOLanguageCode("Moksha", null, "mdf"),
                new ISOLanguageCode("Mandar", null, "mdr"),
                new ISOLanguageCode("Mende", null, "men"),
                new ISOLanguageCode("Irish, Middle (900-1200)", null, "mga"),
                new ISOLanguageCode("Mi'kmaq; Micmac", null, "mic"),
                new ISOLanguageCode("Minangkabau", null, "min"),
                new ISOLanguageCode("Uncoded languages", null, "mis"),
                new ISOLanguageCode("Mon-Khmer languages", null, "mkh"),
                new ISOLanguageCode("Malagasy", "mg", "mlg"),
                new ISOLanguageCode("Maltese", "mt", "mlt"),
                new ISOLanguageCode("Manchu", null, "mnc"),
                new ISOLanguageCode("Manipuri", null, "mni"),
                new ISOLanguageCode("Manobo languages", null, "mno"),
                new ISOLanguageCode("Mohawk", null, "moh"),
                new ISOLanguageCode("Mongolian", "mn", "mon"),
                new ISOLanguageCode("Mossi", null, "mos"),
                new ISOLanguageCode("Multiple languages", null, "mul"),
                new ISOLanguageCode("Munda languages", null, "mun"),
                new ISOLanguageCode("Creek", null, "mus"),
                new ISOLanguageCode("Mirandese", null, "mwl"),
                new ISOLanguageCode("Marwari", null, "mwr"),
                new ISOLanguageCode("Mayan languages", null, "myn"),
                new ISOLanguageCode("Erzya", null, "myv"),
                new ISOLanguageCode("Nahuatl languages", null, "nah"),
                new ISOLanguageCode("North American Indian languages", null, "nai"),
                new ISOLanguageCode("Neapolitan", null, "nap"),
                new ISOLanguageCode("Nauru", "na", "nau"),
                new ISOLanguageCode("Navajo, Navaho", "nv", "nav"),
                new ISOLanguageCode("nr", "nbl", null, "Ndebele, South", "South Ndebele"),
                new ISOLanguageCode("nd", "nde", null, "Ndebele, North", "North Ndebele"),
                new ISOLanguageCode("Ndonga", "ng", "ndo"),
                new ISOLanguageCode(null, "nds", null, "Low German", "Low Saxon", "Saxon", "Low"),
                new ISOLanguageCode("Nepali", "ne", "nep"),
                new ISOLanguageCode(null, "new", null, "Nepal Bhasa", "Newari"),
                new ISOLanguageCode("Nias", null, "nia"),
                new ISOLanguageCode("Niger-Kordofanian languages", null, "nic"),
                new ISOLanguageCode("Niuean", null, "niu"),
                new ISOLanguageCode("nn", "nno", null, "Norwegian Nynorsk", "Nynorsk"),
                new ISOLanguageCode("nb", "nob", null, "Norwegian Bokmål", "Bokmål"),
                new ISOLanguageCode("Nogai", null, "nog"),
                new ISOLanguageCode("Norse, Old", null, "non"),
                new ISOLanguageCode("Norwegian", "no", "nor"),
                new ISOLanguageCode("N'Ko", null, "nqo"),
                new ISOLanguageCode(null, "nso", "Pedi", "Sepedi", "Northern Sotho"),
                new ISOLanguageCode("Nubian languages", null, "nub"),
                new ISOLanguageCode(null, "nwc", null, "Classical Newari", "Old Newari", "Classical Nepal Bhasa"),
                new ISOLanguageCode("ny", "nya", null, "Chichewa", "Chewa", "Nyanja"),
                new ISOLanguageCode("Nyamwezi", null, "nym"),
                new ISOLanguageCode("Nyankole", null, "nyn"),
                new ISOLanguageCode("Nyoro", null, "nyo"),
                new ISOLanguageCode("Nzima", null, "nzi"),
                new ISOLanguageCode("Occitan (post 1500)", "oc", "oci"),
                new ISOLanguageCode("Ojibwa", "oj", "oji"),
                new ISOLanguageCode("Oriya", "or", "ori"),
                new ISOLanguageCode("Oromo", "om", "orm"),
                new ISOLanguageCode("Osage", null, "osa"),
                new ISOLanguageCode("os", "oss", null, "Ossetian", "Ossetic"),
                new ISOLanguageCode("Turkish, Ottoman (1500-1928)", null, "ota"),
                new ISOLanguageCode("Otomian languages", null, "oto"),
                new ISOLanguageCode("Papuan languages", null, "paa"),
                new ISOLanguageCode("Pangasinan", null, "pag"),
                new ISOLanguageCode("Pahlavi", null, "pal"),
                new ISOLanguageCode(null, "pam", null, "Pampanga", "Kapampangan"),
                new ISOLanguageCode("pa", "pan", null, "Panjabi", "Punjabi"),
                new ISOLanguageCode("Papiamento", null, "pap"),
                new ISOLanguageCode("Palauan", null, "pau"),
                new ISOLanguageCode("Persian, Old (ca.600-400 B.C.)", null, "peo"),
                new ISOLanguageCode("Persian", "fa", "per", "fas"),
                new ISOLanguageCode("Philippine languages", null, "phi"),
                new ISOLanguageCode("Phoenician", null, "phn"),
                new ISOLanguageCode("Pali", "pi", "pli"),
                new ISOLanguageCode("Polish", "pl", "pol"),
                new ISOLanguageCode("Pohnpeian", null, "pon"),
                new ISOLanguageCode("Portuguese", "pt", "por"),
                new ISOLanguageCode("Prakrit languages", null, "pra"),
                new ISOLanguageCode("Provençal, Old (to 1500);Occitan, Old (to 1500)", null, "pro"),
                new ISOLanguageCode("ps", "pus", null, "Pushto", "Pashto"),
                new ISOLanguageCode("Quechua", "qu", "que"),
                new ISOLanguageCode("Rajasthani", null, "raj"),
                new ISOLanguageCode("Rapanui", null, "rap"),
                new ISOLanguageCode(null, "rar", null, "Rarotongan", "Cook Islands Maori"),
                new ISOLanguageCode("Romance languages", null, "roa"),
                new ISOLanguageCode("Romansh", "rm", "roh"),
                new ISOLanguageCode("Romany", null, "rom"),
                new ISOLanguageCode("ro", "rum", "ron", "Romanian", "Moldavian", "Moldovan"),
                new ISOLanguageCode("Rundi", "rn", "run"),
                new ISOLanguageCode(null, "rup", null, "Aromanian", "Arumanian", "Macedo-Romanian"),
                new ISOLanguageCode("Russian", "ru", "rus"),
                new ISOLanguageCode("Sandawe", null, "sad"),
                new ISOLanguageCode("Sango", "sg", "sag"),
                new ISOLanguageCode("Yakut", null, "sah"),
                new ISOLanguageCode("South American Indian languages", null, "sai"),
                new ISOLanguageCode("Salishan languages", null, "sal"),
                new ISOLanguageCode("Samaritan Aramaic", null, "sam"),
                new ISOLanguageCode("Sanskrit", "sa", "san"),
                new ISOLanguageCode("Sasak", null, "sas"),
                new ISOLanguageCode("Santali", null, "sat"),
                new ISOLanguageCode("Sicilian", null, "scn"),
                new ISOLanguageCode("Scots", null, "sco"),
                new ISOLanguageCode("Selkup", null, "sel"),
                new ISOLanguageCode("Semitic languages", null, "sem"),
                new ISOLanguageCode("Irish, Old (to 900)", null, "sga"),
                new ISOLanguageCode("Sign Languages", null, "sgn"),
                new ISOLanguageCode("Shan", null, "shn"),
                new ISOLanguageCode("Sidamo", null, "sid"),
                new ISOLanguageCode("si", "sin", null, "Sinhala", "Sinhalese"),
                new ISOLanguageCode("Siouan languages", null, "sio"),
                new ISOLanguageCode("Sino-Tibetan languages", null, "sit"),
                new ISOLanguageCode("Slavic languages", null, "sla"),
                new ISOLanguageCode("Slovak", "sk", "slo", "slk"),
                new ISOLanguageCode("Slovenian", "sl", "slv"),
                new ISOLanguageCode("Southern Sami", null, "sma"),
                new ISOLanguageCode("Northern Sami", "se", "sme"),
                new ISOLanguageCode("Sami languages", null, "smi"),
                new ISOLanguageCode("Lule Sami", null, "smj"),
                new ISOLanguageCode("Inari Sami", null, "smn"),
                new ISOLanguageCode("Samoan", "sm", "smo"),
                new ISOLanguageCode("Skolt Sami", null, "sms"),
                new ISOLanguageCode("Shona", "sn", "sna"),
                new ISOLanguageCode("Sindhi", "sd", "snd"),
                new ISOLanguageCode("Soninke", null, "snk"),
                new ISOLanguageCode("Sogdian", null, "sog"),
                new ISOLanguageCode("Somali", "so", "som"),
                new ISOLanguageCode("Songhai languages", null, "son"),
                new ISOLanguageCode("Sotho, Southern", "st", "sot"),
                new ISOLanguageCode("es", "spa", null, "Spanish", "Espańol", "Espanol", "Castilian"),
                new ISOLanguageCode("Sardinian", "sc", "srd"),
                new ISOLanguageCode("Sranan Tongo", null, "srn"),
                new ISOLanguageCode("Serbian", "sr", "srp"),
                new ISOLanguageCode("Serer", null, "srr"),
                new ISOLanguageCode("Nilo-Saharan languages", null, "ssa"),
                new ISOLanguageCode("Swati", "ss", "ssw"),
                new ISOLanguageCode("Sukuma", null, "suk"),
                new ISOLanguageCode("Sundanese", "su", "sun"),
                new ISOLanguageCode("Susu", null, "sus"),
                new ISOLanguageCode("Sumerian", null, "sux"),
                new ISOLanguageCode("Swahili", "sw", "swa"),
                new ISOLanguageCode("Swedish", "sv", "swe"),
                new ISOLanguageCode("Classical Syriac", null, "syc"),
                new ISOLanguageCode("Syriac", null, "syr"),
                new ISOLanguageCode("Tahitian", "ty", "tah"),
                new ISOLanguageCode("Tai languages", null, "tai"),
                new ISOLanguageCode("Tamil", "ta", "tam"),
                new ISOLanguageCode("Tatar", "tt", "tat"),
                new ISOLanguageCode("Telugu", "te", "tel"),
                new ISOLanguageCode("Timne", null, "tem"),
                new ISOLanguageCode("Tereno", null, "ter"),
                new ISOLanguageCode("Tetum", null, "tet"),
                new ISOLanguageCode("Tajik", "tg", "tgk"),
                new ISOLanguageCode("Tagalog", "tl", "tgl"),
                new ISOLanguageCode("Thai", "th", "tha"),
                new ISOLanguageCode("Tibetan", "bo", "tib", "bod"),
                new ISOLanguageCode("Tigre", null, "tig"),
                new ISOLanguageCode("Tigrinya", "ti", "tir"),
                new ISOLanguageCode("Tiv", null, "tiv"),
                new ISOLanguageCode("Tokelau", null, "tkl"),
                new ISOLanguageCode(null, "tlh", null, "Klingon", "tlhIngan-Hol"),
                new ISOLanguageCode("Tlingit", null, "tli"),
                new ISOLanguageCode("Tamashek", null, "tmh"),
                new ISOLanguageCode("Tonga (Nyasa)", null, "tog"),
                new ISOLanguageCode("Tonga (Tonga Islands)", "to", "ton"),
                new ISOLanguageCode("Tok Pisin", null, "tpi"),
                new ISOLanguageCode("Tsimshian", null, "tsi"),
                new ISOLanguageCode("Tswana", "tn", "tsn"),
                new ISOLanguageCode("Tsonga", "ts", "tso"),
                new ISOLanguageCode("Turkmen", "tk", "tuk"),
                new ISOLanguageCode("Tumbuka", null, "tum"),
                new ISOLanguageCode("Tupi languages", null, "tup"),
                new ISOLanguageCode("Turkish", "tr", "tur"),
                new ISOLanguageCode("Altaic languages", null, "tut"),
                new ISOLanguageCode("Tuvalu", null, "tvl"),
                new ISOLanguageCode("Twi", "tw", "twi"),
                new ISOLanguageCode("Tuvinian", null, "tyv"),
                new ISOLanguageCode("Udmurt", null, "udm"),
                new ISOLanguageCode("Ugaritic", null, "uga"),
                new ISOLanguageCode("Uighur, Uyghur", "ug", "uig"),
                new ISOLanguageCode("Ukrainian", "uk", "ukr"),
                new ISOLanguageCode("Umbundu", null, "umb"),
                new ISOLanguageCode("Undetermined", null, "und"),
                new ISOLanguageCode("Urdu", "ur", "urd"),
                new ISOLanguageCode("Uzbek", "uz", "uzb"),
                new ISOLanguageCode("Vai", null, "vai"),
                new ISOLanguageCode("Venda", "ve", "ven"),
                new ISOLanguageCode("Vietnamese", "vi", "vie"),
                new ISOLanguageCode("Volapük", "vo", "vol"),
                new ISOLanguageCode("Votic", null, "vot"),
                new ISOLanguageCode("Wakashan languages", null, "wak"),
                new ISOLanguageCode(null, "wal", null, "Wolaitta", "Wolaytta"),
                new ISOLanguageCode("Waray", null, "war"),
                new ISOLanguageCode("Washo", null, "was"),
                new ISOLanguageCode("Welsh", "cy", "wel", "cym"),
                new ISOLanguageCode("Sorbian languages", null, "wen"),
                new ISOLanguageCode("Walloon", "wa", "wln"),
                new ISOLanguageCode("Wolof", "wo", "wol"),
                new ISOLanguageCode(null, "xal", null, "Kalmyk", "Oirat"),
                new ISOLanguageCode("Xhosa", "xh", "xho"),
                new ISOLanguageCode("Yao", null, "yao"),
                new ISOLanguageCode("Yapese", null, "yap"),
                new ISOLanguageCode("Yiddish", "yi", "yid"),
                new ISOLanguageCode("Yoruba", "yo", "yor"),
                new ISOLanguageCode("Yupik languages", null, "ypk"),
                new ISOLanguageCode("Zapotec", null, "zap"),
                new ISOLanguageCode(null, "zbl", null, "Blissymbols", "Blissymbolics", "Bliss"),
                new ISOLanguageCode("Zenaga", null, "zen"),
                new ISOLanguageCode("Standard Moroccan Tamazight", null, "zgh"),
                new ISOLanguageCode("za", "zha", null, "Zhuang", "Chuang"),
                new ISOLanguageCode("Zande languages", null, "znd"),
                new ISOLanguageCode("Zulu", "zu", "zul"),
                new ISOLanguageCode("Zuni", null, "zun"),
                new ISOLanguageCode(null, "zza", null, "Zaza", "Dimili", "Dimli", "Kirdki", "Kirmanjki", "Zazaki"),

                #endregion
            };

            foreach (ISOLanguageCode languageCode in ilc) {
                //if 2 letter code exists
                if (languageCode.Alpha2 != null) {
                    Codes.Add(languageCode.Alpha2, languageCode);
                }

                //if the language has both Terminology and Bibliographic 3 letter code
                if (languageCode.Alpha3Terminology != null) {
                    Codes.Add(languageCode.Alpha3Terminology, languageCode);
                }

                Codes.Add(languageCode.Alpha3, languageCode);

                foreach (string languageName in languageCode.Languages) {
                    LanguageNames.Add(languageName, languageCode.Alpha3);
                }
            }
        }

        private ISOLanguageCodes() {
        }

        public static ISOLanguageCodes Instance { get { return _instance; } }

        /// <summary>Checks if the specified string is a valid ISO English langauge name.</summary>
        /// <param name="name">The language name to check.</param>
        /// <returns>Returns <c>true</c> if the name is valid; otherwise <c>false</c>.</returns>
        public bool IsAnISOEnglishLanguageName(string name) {
            return LanguageNames.ContainsKey(name);
        }

        /// <summary>Gets the ISO Language code with specified english name.</summary>
        /// <param name="name">The english name.</param>
        /// <returns>An instance of <see cref="ISOCode"/> if found; otherwise <c>null</c>.</returns>
        public override ISOLanguageCode GetByEnglishName(string name) {
            string langCode;
            return LanguageNames.TryGetValue(name, out langCode) ? Codes[langCode] : null;
        }

        /// <summary>Gets all known ISO codes.</summary>
        /// <returns>An information about all known ISO codes.</returns>
        public new static IEnumerable<ISOLanguageCode> GetAllKnownCodes() {
            return Codes.Values.Distinct();
        }
    }

}