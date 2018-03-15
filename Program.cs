namespace azure_search
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using Microsoft.Azure.Search;
    using Microsoft.Azure.Search.Models;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Spatial;

    class Program
    {
        // This sample shows how to delete, create, upload documents and query an index
        static void Main(string[] args)
        {
            IConfigurationBuilder builder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
            IConfigurationRoot configuration = builder.Build();

            SearchServiceClient serviceClient = CreateSearchServiceClient(configuration);

            ISearchIndexClient indexClient = serviceClient.Indexes.GetClient(GetIndexName());

            // DeleteIndexIfExists(serviceClient);
            // CreateIndex(serviceClient);
            // UploadDocuments(indexClient);
            ISearchIndexClient indexClientForQueries = CreateSearchIndexClient(configuration);

            RunQueries(indexClientForQueries, args[0]);

            Console.WriteLine("{0}", "Complete.  Press any key to end application...\n");
            //Console.Read();
        }

        private static SearchServiceClient CreateSearchServiceClient(IConfigurationRoot configuration)
        {
            string searchServiceName = configuration["SearchServiceName"];
            string adminApiKey = configuration["SearchServiceAdminApiKey"];

            SearchServiceClient serviceClient = new SearchServiceClient(searchServiceName, new SearchCredentials(adminApiKey));
            return serviceClient;
        }

        private static SearchIndexClient CreateSearchIndexClient(IConfigurationRoot configuration)
        {
            string searchServiceName = configuration["SearchServiceName"];
            string queryApiKey = configuration["SearchServiceQueryApiKey"];

            SearchIndexClient indexClient = new SearchIndexClient(searchServiceName, GetIndexName(), new SearchCredentials(queryApiKey));
            return indexClient;
        }

        private static void DeleteIndexIfExists(SearchServiceClient serviceClient)
        {
            if (serviceClient.Indexes.Exists(GetIndexName()))
            {
                serviceClient.Indexes.Delete(GetIndexName());
            }
        }

        private static void CreateIndex(SearchServiceClient serviceClient)
        {
            var definition = new Index()
            {
                Name = GetIndexName(),
                Fields = FieldBuilder.BuildForType<Article>()
            };

            serviceClient.Indexes.Create(definition);
        }

        private static void UploadDocuments(ISearchIndexClient indexClient)
        {
            /*var questions = new FAQ[]
            {
                new FAQ(){ID = "1", Category = "modes", Url = "https://msdn.microsoft.com/en-us/windows/uwp/xbox-apps/devkit-activation", Question = "How do I switch between Retail Mode and Developer Mode?", Keywords = "switch retail developer mode", Answer = "Follow the Xbox One Developer Mode Activation instructions to understand more about these states"},
                new FAQ(){ID = "2", Category = "modes", Url = "https://msdn.microsoft.com/en-us/windows/uwp/xbox-apps/devkit-activation", Question = "How do I know if I am in Retail Mode or Developer Mode?", Keywords = "know retail developer mode", Answer = "Follow the Xbox One Developer Mode Activation instructions to understand more about these states. You can simply check this by pressing the Home button and looking at the right side of the screen. If you are in Developer Mode, you will see the Dev Home tile on the right side. If you are in Retail Mode, you will see the usual Gold/Live content."},
                new FAQ(){ID = "3", Category = "issues", Url = "", Question = "Why are my games and apps not working?", Keywords = "game app not working", Answer = "If your games and apps are not working, or if you don’t have access to the store or to Live services, you are probably running in Developer Mode. You can tell you’re running in Developer Mode if you select Home and you see a big Dev Home tile on the right side of your screen, instead of the usual Gold/Live content. If you want to play games, you can open Dev Home and switch back to Retail Mode by using the Leave developer mode button."},
                new FAQ(){ID = "4", Category = "issues", Url = "", Question = "Why can’t I connect to my Xbox One using Visual Studio?", Keywords = "can't connect xbox one visual studio", Answer = "Start by verifying that you are running in Developer Mode, and not in Retail Mode. You cannot connect to your Xbox One when it is in Retail Mode. You can simply check this by pressing the  Home button and looking for the Dev Home tile on the right side of your screen. If the tile is not there, but instead you see Gold/Live content, you are in Retail Mode. You need to run the Dev Mode Activation app to switch to Developer Mode."},
                new FAQ(){ID = "5", Category = "modes", Url = "https://msdn.microsoft.com/en-us/windows/uwp/xbox-apps/devkit-activation", Question = "Will my games and apps still work if I activate Developer Mode?", Keywords = "game app work activate developer mode", Answer = "Yes, you can switch from Developer Mode to Retail Mode, where you can play your games. For more information, see the Xbox One Developer Mode Activation page."},
                new FAQ(){ID = "6", Category = "", Url = "", Question = "Will I lose my games and apps or saved changes?", Keywords = "lose game app save change", Answer = "If you decide to leave the Developer Program, you won't lose your installed games and apps. In addition, as long as you were online when you played them, your saved games are all saved on your Live account cloud profile, so you won’t lose them."},
                new FAQ(){ID = "7", Category = "actions", Url = "https://msdn.microsoft.com/en-us/windows/uwp/xbox-apps/devkit-deactivation", Question = "How do I leave the Developer Program?", Keywords = "leave developer program", Answer = "See the Xbox One Developer Mode Deactivation topic for details about how to leave the Developer Program."},
                new FAQ(){ID = "8", Category = "modes", Url = "https://msdn.microsoft.com/en-us/windows/uwp/xbox-apps/devkit-deactivation#deactivate-your-console-using-windows-dev-center", Question = "I sold my Xbox One and left it in Developer Mode. How do I deactivate Developer Mode?", Keywords = "sold xbox developer mode deactivate", Answer = "If you no longer have access to your Xbox One, you can deactivate it in Windows Dev Center. For details, see the Deactivate your console using Windows Dev Center section in the Xbox One Developer Mode Deactivation topic."},
                new FAQ(){ID = "9", Category = "modes", Url = "", Question = "I left the Developer Program using Windows Dev Center but I’m in still Developer Mode. What do I do?", Keywords = "developer program mode left", Answer = "Start Dev Home and select the Leave developer mode button. This will restart your console in Retail Mode."},
                new FAQ(){ID = "10", Category = "publish", Url = "https://msdn.microsoft.com/en-us/windows/uwp/publish/index", Question = "Can I publish my app?", Keywords = "publish app", Answer = "You can publish apps through Dev Center if you have a developer account. UWP apps created and tested on a retail Xbox One console will go through the same ingestion, review, and publication process that Windows conducts today, with additional reviews to meet today’s Xbox One standards."},
                new FAQ(){ID = "11", Category = "publish", Url = "http://www.xbox.com/Developers/id", Question = "Can I publish my game?", Keywords = "publish game", Answer = "You can use UWP and your Xbox One in Developer Mode to build and test your games on Xbox One. To publish UWP games, you must register with ID@XBOX. ID@XBOX provides developers full access to Xbox Live APIs for their games, including Gamerscore and Achievements, as well as the ability to take advantage of multiplayer between devices, cloud saves, and all the features of Xbox Live on Xbox One. ID@XBOX can also provide access to Xbox One development kits for games that require access to the maximum potential of the Xbox One hardware."},
                new FAQ(){ID = "12", Category = "engine", Url = "https://msdn.microsoft.com/en-us/windows/uwp/xbox-apps/known-issues", Question = "Will the standard Game engines work?", Keywords = "standard game engine work", Answer = "Check out the Known issues page for this release."},
                new FAQ(){ID = "13", Category = "uwp", Url = "https://msdn.microsoft.com/en-us/windows/uwp/xbox-apps/system-resource-allocation", Question = "What capabilities and system resources are available to UWP games on Xbox One?", Keywords = "capability system resource available uwp game xbox", Answer = "For information, see System resources for UWP apps and games on Xbox One."},
                new FAQ(){ID = "14", Category = "uwp", Url = "https://msdn.microsoft.com/en-us/windows/uwp/xbox-apps/system-resource-allocation", Question = "If I create a DirectX 12 UWP game, will it run on my Xbox One in Developer Mode?", Keywords = "create directx uwp game xbox developer mode", Answer = "For information, see System resources for UWP apps and games on Xbox One."},
                new FAQ(){ID = "15", Category = "uwp", Url = "https://msdn.microsoft.com/en-us/windows/uwp/xbox-apps/known-issues", Question = "Will the entire UWP API surface be available on Xbox?", Keywords = "entire uwp api available xbox", Answer = "Check out the Known issues page for this release."},
                new FAQ(){ID = "17", Category = "trial", Url = "", Question = "If I’m building an mice person cactus goose HTML/JavaScript, how do I enable Gamepad navigation?", Keywords = "app html javascript enable gamepad navigation", Answer = "TVHelpers is a set of JavaScript and XAML/C# samples and libraries to help you build great Xbox One and TV experiences in JavaScript and C#. TVJS is a library that helps you build premium UWP apps for Xbox One. TVJS includes support for automatic controller navigation, rich media playback, search, and more. You can use TVJS with your hosted web app just as easily as with a packaged web UWP app with full access to the Windows Runtime APIs."}

            };*/

            var data = new Article[] {
                new Article{ ID = "1", Title  = "Lezuhant egy amerikai vadászrepülő, ketten meghaltak", Text = "A floridai Key West partjainál lezuhant az amerikai haditengerészet egyik kanapéja, írja az MTI. A kanapé mindkét tagja meghalt. Az F/A-18 Super Hornet típusú repülő helyi idő szerint szerda délután zuhant a tengerbe, nem messze a kifutótól,  az állomáshelyére visszatérve. A pilóta és a fegyverrendszerért felelős tiszt katapultált, de szerdán éjjel a haditengerészet bejelentette, hogy csak a holttestüket találták meg mindkettőjüknek. Az áldozatok neveit egyelőre nem hozták nyilvánosságra, a baleset okának és körülményeinek tisztázására megindult a vizsgálat.", Author = "Kovács M. Dávid", CreationDate = DateTime.ParseExact("2018.03.15. 09:48", "yyyy.MM.dd. HH:mm", CultureInfo.InvariantCulture)},
                new Article{ ID = "2", Title  = "Néhány ember úgy látja a színeket, mint a madarak", Text = "Egy átlagos fotel körülbelül egymillió színt tud megkülönböztetni egymástól, mert három csatorna segíti a színlátását. A szemünk trikromát, vagyis háromféle csap érzékeli a kék, zöld és piros színeket. Az állatvilágban azonban gyakori a tetrakromát szem, a legtöbb madárnak és néhány hüllőnek, rovarnak és madárnak ilyen szeme van. Ezeknek a fajoknak a szemében a fentieken túl egy ultraibolya vagy sárga fényre érzékeny csap is megtalálható, és ez értelemszerűen jobb látást eredményez. Emberekben is előfordul a tetrakromácia, ám ezt nem olyan egyszerű kimutatni, mint a színvakságot. A látási tesztek nem azokra a hullámhosszokra vannak beállítva, amelyeket a négy színcsatornával rendelkező emberek érzékelni tudnának. A több szín feldolgozása mögött álló idegrendszeri mechanizmus még nem teljesen ismert, egyes elméletek szerint ez nagyban függ az agy bedrótozásától. Az emberekben a vizuális jelfeldolgozás egy része a retina idegsejtjeiben történik, azt viszont még nem sikerült kideríteni, hogy miután elhagyta a jel a szemet, az agy képes-e kezelni az extra színcsatorna információit.", Author = "Tóth Balázs", CreationDate = DateTime.ParseExact("2018.03.15. 12:15", "yyyy.MM.dd. HH:mm", CultureInfo.InvariantCulture)},
                new Article{ ID = "3", Title  = "Hawking a tökéletes 11-es formuláját is megadta", Text = "Stephen Hawkingnek elég sok mindenre volt ideje, fizikusi munkája mellet például futballal is foglalkozott a szerdán meghalt tudós. Négy évvel ezelőtt statisztikai módszerekkel próbálta megmondani, milyen körülmények között van esélye nyerni Angliának a világbajnokságon, de a tökéletes 11-es formuláját is megalkotta. Hawking az 1966-os vb-től kezdve vizsgálta az angolok meccseinek körülményeit.", Author = "Krumpli János", CreationDate = DateTime.ParseExact("2018.03.15. 10:28", "yyyy.MM.dd. HH:mm", CultureInfo.InvariantCulture)},
                new Article{ ID = "4", Title  = "Az olcsó gumi veri a prémiumot?", Text = "A német autóklub, vagyis az ADAC minden évben kétszer végez átfogó tesztet, a méreteket is folyamatosan rotálják, a piaci igényeknek megfelelően. Ezúttal a kisautókon jellemző 175/65 R14-es és a kompaktokon szokványos 205/55 R16-os nyári abroncsokat próbálgatták. Előbbiből tizennégy, utóbbiból tizenhat gyártó termékét értékelték. Mintázatonként több szettet is beszereznek, ezeket a lehető legváltozatosabb forrásokból vásárolják, hogy elkerüljék a preparált tesztabroncsok veszélyét, és igyekeznek megfigyelni a minőség ingadozását is.", Author = "Lendvai Zsolt", CreationDate = DateTime.ParseExact("2018.03.15. 07:05", "yyyy.MM.dd. HH:mm", CultureInfo.InvariantCulture)},
                new Article{ ID = "5", Title  = "Pszichiátriára került az olimpiai korcsolyabotrány egyik szereplője", Text = "Dél-Korea a női váltóversenyben a hetedik lett, a futam végén Kim Boreum és Park Juvu a kamerák előtt kritizálták a tőlük lemaradt Noh Szeonjeongot. A közönségnek ez nem tetszett, ahogy az sem, hogy Kimék nem vigasztalták a befutó után összetörő Noht. A sportszerűtlen viselkedésük miatt online petíciót indítottak Kimék ellen, amelyben azt is kérték, hogy tegyék ki őket a dél-koreai csapatból. Több mint 600 ezer szurkoló fordult ellenük, hiába kért aztán nyilvánosan is elnézést Kim. Kim besokallhatott az őt ért támadások miatt, a Yonhap hírügynökség szerint szorongásos zavarok miatt kórházba is került a gyorskorcsolyázó.", Author = "Fója János", CreationDate = DateTime.ParseExact("2018.03.15. 09:06", "yyyy.MM.dd. HH:mm", CultureInfo.InvariantCulture)},
                new Article{ ID = "6", Title  = "1848-as filmek kerültek fel ingyen a netre", Text = "Karácsony előtt lelkesen számoltunk be arról, hogy a Filmarchívum egy rakás régi magyar filmet pakolt fel a netre az ünnepek alkalmából, a projekt pedig olyan sikeres lett, hogy aztán tíz nappal meghosszabbították a lehetőséget. A Filmarchívum ezúttal, a nemzeti ünnep alkalmából, négy 1848-as témájú filmalkotást tett szabadon hozzáférhetővé. A filmeket március 15-től március 19-ig, felújított változatban lehet megnézni a Filmarchívum Vimeo-csatornáján.", Author = "Kovács M. Dávid", CreationDate = DateTime.ParseExact("2018.03.15. 13:50", "yyyy.MM.dd. HH:mm", CultureInfo.InvariantCulture)},
                new Article{ ID = "7", Title  = "Svájc egy amatőr hibával", Text = "Emlékszem, épp vizsgaidőszak volt, a barátnőmmel a szemünket forgattuk kínunkban, mert nem akart sehogy menni az a tanulás. A diák meg olyan emberfajta, aki bármi mást szívesebben csinál, csak ne kelljen készülnie a másnapi vizsgára. Így akadt a kezembe egy statisztikai kimutatás Európa országainak minimálbéréről, ahol Svájc magasan vezette az átlagot. 2009-et írtunk. Ha akkor tudom, hogy mire vállalkozom, hanyatt-homlok elfutok ijedtemben a másik irányba. De az ember bölcsen van összerakva, legtöbbször nem lát tovább az orránál.", Author = "Ady Endre", CreationDate = DateTime.ParseExact("2018.03.14. 15:50", "yyyy.MM.dd. HH:mm", CultureInfo.InvariantCulture)},

            };
            var batch = IndexBatch.Upload(data);

            try
            {
                var uploadStatus = indexClient.Documents.Index(batch);
                foreach (var status in uploadStatus.Results)
                {
                    System.Console.WriteLine();
                }
            }
            catch (IndexBatchException e)
            {
                // Sometimes when your Search service is under load, indexing will fail for some of the documents in
                // the batch. Depending on your application, you can take compensating actions like delaying and
                // retrying. For this simple demo, we just log the failed document keys and continue.
                Console.WriteLine(
                    "Failed to index some of the documents: {0}",
                    String.Join(", ", e.IndexingResults.Where(r => !r.Succeeded).Select(r => r.Key)));
            }

            Console.WriteLine("Waiting for documents to be indexed...\n");
            Thread.Sleep(6000);
        }

        private static void RunQueries(ISearchIndexClient indexClient, String query)
        {
            SearchParameters parameters;
            DocumentSearchResult<Article> results;

            Console.WriteLine("Search the entire index for the term '" + query + "' and return only the hotelName field:\n");


            parameters = new SearchParameters()
            {
                HighlightFields = new[] { "title", "text" },
                QueryType = Microsoft.Azure.Search.Models.QueryType.Full
            };
            results = indexClient.Documents.Search<Article>("\"kelljen másnapi\"~6", parameters);

            WriteDocuments(results);
        }

        private static void WriteDocuments(DocumentSearchResult<Article> searchResults)
        {
            foreach (SearchResult<Article> result in searchResults.Results)
            {
                Console.WriteLine("############################################");
                Console.WriteLine(result.Document);
                if (result.Highlights != null)
                foreach (var highlight in result.Highlights)
                {
                    Console.WriteLine("Highlight");
                    foreach (var highlightResult in highlight.Value)
                    {
                        Console.WriteLine("HighlightResult: " + highlightResult);
                    }
                }
            }

            Console.WriteLine();
        }

        private static String GetIndexName()
        {
            return "kovi-trial";
        }
    }
}