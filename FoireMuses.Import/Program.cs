using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MusicDatabase2.Services;
using System.Text.RegularExpressions;
using LoveSeat;
using MusicDatabase2.Business;
using MindTouch.Xml;
using System.IO;
using MindTouch.Dream;
using Newtonsoft.Json.Linq;
using MindTouch.Tasking;

namespace FoireMuses.Import
{
    class Program
    {
        static void Main(string[] args)
        {
            ControllerFactory.Instance.Initialize(@"c:\Projects\MusicDatabaseData\");
            CouchClient client = new CouchClient();
            CouchDatabase db = client.GetDatabase("musicdatabasexml");



            foreach (ISource source in ControllerFactory.Instance.SourceController.GetAllSources(false))
            {
                JDocument doc = new JDocument();
                doc.Id = source.Id;
                doc["otype"] = "source";
                AddProperty(doc, source.Abbreviation, "abbr");
                AddProperty(doc, source.Cote, "cote");
                AddProperty(doc, source.DateFrom, "dateFrom");
                AddProperty(doc, source.DateTo, "dateTo");
                AddProperty(doc, source.FreeZone, "free");
                AddProperty(doc, source.IsApproxDate, "approxDate");
                AddProperty(doc, source.IsMusicalSource, "musicalSource");
                AddProperty(doc, source.Name, "name");
                AddProperty(doc, source.Publisher, "publisher");

                db.CreateDocument(doc);
            }




            foreach (IPlay play in ControllerFactory.Instance.PlayController.GetAllPlays())
            {


                JDocument doc = new JDocument();
                doc.Id = play.Id;
                doc["otype"] = "play";
                AddProperty(doc, play.Abstract, "abstract");
                AddProperty(doc, play.ActionLocation, "actionLocation");
                AddProperty(doc, play.Author, "author");
                AddProperty(doc, play.ContemporaryComments, "comments");
                AddProperty(doc, play.CreationPlace, "creationPlace");
                AddProperty(doc, play.CreationYear, "creationYear");
                AddProperty(doc, play.Critics, "critics");
                AddProperty(doc, play.Decors, "decors");
                //AddProperty(doc, play.Distribution, "distribution");
                AddProperty(doc, play.EntrepreneurName, "entrepreneurName");
                AddProperty(doc, play.Genre, "type");
                AddProperty(doc, play.Iconography, "iconography");
                AddProperty(doc, play.MusicianName, "musicianName");
                //AddProperty(doc, play.NoticeDeRepresentation, "notice");
                AddProperty(doc, play.Resonances, "resonances");
                AddProperty(doc, play.SourceFolio, "sourceFolio");
                AddProperty(doc, play.SourceTome, "sourceTome");
                AddProperty(doc, play.SourceVolume, "sourceVolume");
                AddProperty(doc, play.Title, "title");
                AddProperty(doc, play.TotalActs, "totalActs");//REALLY USEFULL?
                AddProperty(doc, play.TotalScenes, "totalScenes");//REALLY USEFULL?

                ISource source = ControllerFactory.Instance.PlayController.GetSource(play);
                if (source != null)
                {
                    AddProperty(doc, source.Id, "sourceID");
                }



                XDoc xdoc = new XDoc("play");
                AddAttr(xdoc, "actionLocation", play.ActionLocation);
                AddAttr(xdoc, "creationPlace", play.CreationPlace);
                AddAttr(xdoc, "creationYear", play.CreationYear);
                AddAttr(xdoc, "genre", play.Genre);
                AddAttr(xdoc, "entrepreneurName", play.EntrepreneurName);
                AddAttr(xdoc, "musicianName", play.MusicianName);
                AddAttr(xdoc, "title", play.Title);
                AddAttr(xdoc, "sourceFolio", play.SourceFolio);
                AddAttr(xdoc, "sourceTome", play.SourceTome);
                AddAttr(xdoc, "sourceVolume", play.SourceVolume);
                ISource srcc = ControllerFactory.Instance.PlayController.GetSource(play);
                if (srcc != null)
                {
                    AddAttr(xdoc,"sourceID", srcc.Id);
                }

                AddElem(xdoc,"abstract",play.Abstract);
                AddElem(xdoc,"distribution",play.Distribution);
                AddElem(xdoc,"contemporaryComments",play.ContemporaryComments);
                AddElem(xdoc,"critics",play.Critics);
                AddElem(xdoc,"resonance",play.Resonances);
                AddElem(xdoc,"decors",play.Decors);
                AddElem(xdoc,"noticeDeRepresentation",play.NoticeDeRepresentation);                
                
                xdoc.Start("characters");
                foreach (ICharacter character in ControllerFactory.Instance.CharacterController.GetCharacters(play))
                {
                    xdoc.Start("character");
                    AddAttr(xdoc, "id", character.Id);
                    AddAttr(xdoc, "job", character.Job);
                    AddAttr(xdoc, "type", character.Type);
                    AddAttr(xdoc, "model", character.Model);
                    if(!String.IsNullOrEmpty(character.Name))
                        xdoc.Elem("nom", character.Name);
                    xdoc.End();
                }
                xdoc.End(); // end characters


                xdoc.Start("acts");
                foreach (IAct act in ControllerFactory.Instance.PlayController.GetAllActs(play))
                {
                    xdoc.Start("act");
                    AddAttr(xdoc, "number", act.Number);
                    if (act.Type != null)
                    {
                        AddAttr(xdoc, "type", act.Type.ToString());
                    }
                    AddAttr(xdoc, "index", act.Index);


                    AddElem(xdoc, "didascalieAct",act.Didascalie);
               

                    xdoc.Start("scenes");
                    foreach (IScene scene in ControllerFactory.Instance.PlayController.GetAllScenes(act))
                    {
                        xdoc.Start("scene");
                        AddAttr(xdoc, "number", scene.Number);

                        AddElem(xdoc,"didascalieScene",scene.Didascalie);
                        AddElem(xdoc,"distribution",scene.Distribution);

                        xdoc.Start("intervenes");
                        foreach (IIntervene intervene in ControllerFactory.Instance.InterveneController.GetAllIntervenes(scene))
                        {
                            xdoc.Start("intervene");

                            xdoc.Start("participants");
                            foreach (IInterveneCharacter iic in ControllerFactory.Instance.InterveneController.GetCharacters(intervene))
                            {
                                xdoc.Start("participant");
                                AddAttr(xdoc, "charID", iic.Character.Id);
                                AddElem(xdoc,"characterNote",iic.CharacterNote);
                                xdoc.End();
                            }
                            xdoc.End(); // end participants

                            IScore sc = ControllerFactory.Instance.InterveneController.GetAssociatedScore(intervene);
                            if(sc != null)
                                AddAttr(xdoc, "scoreID", sc.Id);

                            AddElem(xdoc,"didascalieIntervene",intervene.Didascalie);
                            if (intervene.Modernization != null)
                            {
                                xdoc.Start("verses");
                                string[] metric={};
                                if (intervene.Transcription != null)
                                {
                                    metric = intervene.Transcription.Split('\n');
                                }
                                int i = 0;
                            
                                foreach (string ligne in intervene.Modernization.Split('\n'))
                                {
                                    xdoc.Start("verse");
                                    Regex reg = new Regex(@"(?<count>\d+)(?<switch>\+\d+|-\d+)?(?<rime>\w)?", RegexOptions.Compiled);
                                    if (metric.Length > i)
                                    {
                                        Match m = reg.Match(metric[i]);
                                        if (m.Success)
                                        {
                                            AddAttr(xdoc, "count", m.Groups[1].Value);
                                            AddAttr(xdoc, "switch", m.Groups[2].Value);
                                            AddAttr(xdoc, "rime", m.Groups[3].Value);
                                        }
                                    }
                                    string maChaine = TraiterChaine(ligne.Trim());
                                    // Console.WriteLine(maChaine.ToString());
                                    XDoc import = XDocFactory.From(maChaine, MimeType.XML);
                                    //Console.Write(import.ToPrettyString());
                                    xdoc.Add(import);

                                    xdoc.End(); // end verse
                                    i++;
                                }
                            
                                //xdoc.Elem("transcription", intervene.Transcription);
                                //xdoc.Elem("modernization", intervene.Modernization);
                                xdoc.End(); // end verses
                            }
                            xdoc.End(); // end intervene
                        }
                        xdoc.End(); // end intervenes
                        xdoc.End(); // end scene
                    }
                    xdoc.End(); //end scenes
                    xdoc.End(); // end act
                }
                xdoc.End();//end acts
                /*StreamWriter sw= File.CreateText("pretty.xml");
                sw.Write(xdoc.ToPrettyString());
                sw.Flush();
                sw.Close();*/
              //  xdoc.Save("monFichier.txt");
                doc = db.CreateDocument(doc);
                using(MemoryStream ms = new MemoryStream(xdoc.ToBytes())){
                    db.AddAttachment(doc.Id, ms,"play.xml");
                }

                string[] files = ControllerFactory.Instance.FileController.GetFiles(play.Id, true);
                foreach (string file in files)
                {
                    Stream monFichier = ControllerFactory.Instance.FileController.GetFile(play.Id, file);
                    string nomFichier = convertirChaineSansAccent(file);
                    Console.WriteLine("Nouveau nom: " + nomFichier);
                    db.AddAttachment(play.Id, monFichier, nomFichier);
                }
                //Console.Write(xdoc.ToJson());

            }  
 

            foreach (IScore score in ControllerFactory.Instance.ScoreController.GetAll())
			{
				Console.WriteLine(score.Id);
				JDocument doc = new JDocument();
				doc.Id = score.Id;
				doc["otype"] = "score";
				AddProperty(doc, score.Title, "title");
				AddProperty(doc, score.Code1, "code1");
				AddProperty(doc, score.Code2 != null ? score.Code2.Replace("-"," -") : null, "code2");
				AddProperty(doc, score.Coirault, "coirault");
				AddProperty(doc, score.Composer, "composer");
				AddProperty(doc, score.CoupeMetrique, "coupeMetrique");
				AddProperty(doc, score.Couplets, "verses");
				AddProperty(doc, score.Delarue, "delarue");
				AddProperty(doc, score.Description + " " + score.FreeZone + " " + score.ReferenceText, "comments");
				AddProperty(doc, score.Editor, "editor");
				AddProperty(doc, score.RythmSignature, "rythmSignature");
				AddProperty(doc, score.OtherTitles, "otherTitles");
				AddProperty(doc, score.Strophe, "stanza");
				AddProperty(doc, score.Type, "type");

				IPlay play = ControllerFactory.Instance.ScoreController.GetSourceAssociee(score);
				if (play != null)
				{
					JObject musicalSource = new JObject();
					AddProperty(musicalSource,play.Id,"id");
					AddProperty(musicalSource,score.SourceAssocieeAct,"act");
					AddProperty(musicalSource,score.SourceAssocieeActId,"actId");
					AddProperty(musicalSource,score.SourceAssocieeAir,"air");
					AddProperty(musicalSource,score.SourceAssocieePage,"page");
					AddProperty(musicalSource,score.SourceAssocieeScene,"scene");
					AddProperty(musicalSource,score.SourceAssocieeSceneId,"sceneId");
					AddProperty(musicalSource,score.SourceAssocieeText,"text");
					AddProperty(musicalSource,score.SuggestedMusicalSource,"isSuggested");

					doc["musicalSource"] = musicalSource;
				}

				ISource source = ControllerFactory.Instance.ScoreController.GetSource(score);
				if (source != null)
				{
					JObject textualSource = new JObject();
					AddProperty(textualSource,source.Id,"id");
					AddProperty(textualSource,score.SourceAir,"air");
					AddProperty(textualSource,score.SourcePage,"page");
					AddProperty(textualSource,score.SourceTome,"tome");
					AddProperty(textualSource,score.SourceVolume,"volume");
					if(score.TextualSourcePageId != Guid.Empty)
						AddProperty(textualSource,score.TextualSourcePageId.ToString("N"),"PageId");

					doc["textualSource"] = textualSource;
				}

				List<string> tags = new List<string>();
				foreach (ITag tag in ControllerFactory.Instance.ScoreController.GetAllTags(score))
				{
					tags.Add(tag.Name.Trim());
				}

				if (tags.Count > 0)
				{
					doc["tags"] = new JArray(tags.ToArray());
				}

				db.CreateDocument(doc);

                string[] files = ControllerFactory.Instance.FileController.GetFiles(score.Id, true);
                foreach (string file in files)
                {
                    Stream monFichier = ControllerFactory.Instance.FileController.GetFile(score.Id, file);
                    string nomFichier = convertirChaineSansAccent(file);
                    db.AddAttachment(score.Id, monFichier, nomFichier);
                }
            }
            db.Compact();
        }

        private static XDoc AddAttr(XDoc xdoc, string name, string value)
        {
            if (!String.IsNullOrEmpty(value))
            {
                xdoc.Attr(name, value);
            }
            return xdoc;
        }


        private static XDoc AddAttr(XDoc xdoc, string name, int value)
        {
            xdoc.Attr(name, value);
            return xdoc;
        }

        private static void AddProperty(JObject doc, string value, string name)
        {
            if (!String.IsNullOrEmpty(value))
                doc[name] = value.Trim();
        }

        private static void AddProperty(JObject doc, int value, string name)
        {
            doc[name] = value;
        }

        private static void AddProperty(JObject doc, bool value, string name)
        {
            doc[name] = value;
        }

        private static string TraiterChaine(string aString)
        {
            if (aString == null ||  aString.Trim() == "") return "";
            //string maReg = "(.*)" + Regex.Escape("[[") + "(did|didc|note|bl|g|s|it) :" + "(.*)" + Regex.Escape("]]") + "(.*)";
            //string maRegAncienneIt = "(.*)" + Regex.Escape("//") + "(.*)" + Regex.Escape("//") + "(.*)";
            //string maRegAncienneGr = "(.*)"+Regex.Escape("**")+"(.*)" + Regex.Escape("**") + "(.*)";
            string stringDeBase = aString;
            // aString = Regex.Replace(aString, "(?<avant>.*)" + Regex.Escape("[[s :")+"(?<elem>["+Regex.Escape("a-zA-Z0-9.+-_|()'!?*:;`,$&²³@éè%^£%`´§#")+"\\s]*]](?<apres>.*)","${avant}<u>${elem}</u>{$apres}");
            aString = Regex.Replace(aString, "(?<avant>.*)" + Regex.Escape("[[s :") + "(?<elem>[^]]*)]](?<apres>.*)", "${avant}<u>${elem}</u>${apres}");
            aString = Regex.Replace(aString, "(?<avant>.*)" + Regex.Escape("[[it :") + "(?<elem>[^]]*)]](?<apres>.*)", "${avant}<i>${elem}</i>${apres}");
            aString = Regex.Replace(aString, "(?<avant>.*)" + Regex.Escape("[[g :") + "(?<elem>[^]]*)]](?<apres>.*)", "${avant}<b>${elem}</b>${apres}");
            aString = Regex.Replace(aString, "(?<avant>.*)" + Regex.Escape("[[did :") + "(?<elem>[^]]*)]](?<apres>.*)", "${avant}<didascalie>${elem}</didascalie>${apres}");
            aString = Regex.Replace(aString, "(?<avant>.*)" + Regex.Escape("[[didc :") + "(?<elem>[^]]*)]](?<apres>.*)", "${avant}<didascalieCentered>${elem}</didascalieCentered>${apres}");
            aString = Regex.Replace(aString, "(?<avant>.*)" + Regex.Escape("[[note :") + "(?<elem>[^]]*)]](?<apres>.*)", "${avant}<note>${elem}</note>${apres}");
            aString = Regex.Replace(aString, "(?<avant>.*)" + Regex.Escape("[[bl :") + "(?<elem>[^]]*)]](?<apres>.*)", "${avant}<bl>${elem}</bl>${apres}");
            aString = Regex.Replace(aString, "(?<avant>.*)" + Regex.Escape("[[Personnage :") + "(?<elem>[^]]*)]](?<apres>.*)", "${avant}<personnageRef>${elem}</personnageRef>${apres}");
            aString = Regex.Replace(aString, "(?<avant>.*)" + Regex.Escape("//") + "(?<elem>[^" + Regex.Escape("/") + "]*)" + Regex.Escape("//") + "(?<apres>.*)", "${avant}<i>${elem}</i>${apres}");
            aString = Regex.Replace(aString, "(?<avant>.*)" + Regex.Escape("**") + "(?<elem>[^" + Regex.Escape("*") + "]*)" + Regex.Escape("**") + "(?<apres>.*)", "${avant}<b>${elem}</b>${apres}");
            if (aString != stringDeBase) return TraiterChaine(aString);
            return "<text>" + aString + "</text>";
        }

        private static XDoc AddElem(XDoc xdoc, string name, string elem)
        {
            if(!String.IsNullOrEmpty(elem)){
                xdoc.Start(name);
                xdoc.Add(XDocFactory.From(TraiterChaine(elem),MimeType.XML));
                xdoc.End();
            }
            return xdoc;
        }
        

        private static string convertirChaineSansAccent(string chaine)
        {
            // Déclaration de variables
            string accent = "ÀÁÂÃÄÅàáâãäåÒÓÔÕÖØòóôõöøÈÉÊËèéêëÌÍÎÏìíîïÙÚÛÜùúûüÿÑñÇç";
            string sansAccent = "AAAAAAaaaaaaOOOOOOooooooEEEEeeeeIIIIiiiiUUUUuuuuyNnCc";

            // Conversion des chaines en tableaux de caractères
            char[] tableauSansAccent = sansAccent.ToCharArray();
            char[] tableauAccent = accent.ToCharArray();

            // Pour chaque accent
            for (int i = 0; i < accent.Length; i++)
            {
                // Remplacement de l'accent par son équivalent sans accent dans la chaîne de caractères
                chaine = chaine.Replace(tableauAccent[i].ToString(), tableauSansAccent[i].ToString());
            }

            // Retour du résultat
            return chaine;
        }
    }
}
