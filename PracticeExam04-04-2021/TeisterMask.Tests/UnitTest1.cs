//Resharper disable InconsistentNaming, CheckNamespace

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using AutoMapper;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using NUnit.Framework;

using TeisterMask;
using TeisterMask.Data;

[TestFixture]
public class Import_000_002
{
    private IServiceProvider serviceProvider;

    private static readonly Assembly CurrentAssembly = typeof(StartUp).Assembly;

    [SetUp]
    public void Setup()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<TeisterMaskProfile>();
        });

        this.serviceProvider = ConfigureServices<TeisterMaskContext>("TeisterMask");
    }

    [Test]
    public void ImportEmployeesZeroTest()
    {
        var context = this.serviceProvider.GetService<TeisterMaskContext>();

        SeedDatabase(context);

        var inputJson =
            "[{\"Username\":\"jstanett0\",\"Email\":\"kknapper0@opera.com\",\"Phone\":\"819-699-1096\",\"Tasks\":[34,32,65,30,30,45,36,67]},{\"Username\":\"mmcellen1\",\"Email\":\"emorten1@ucla.edu\",\"Phone\":\"806-478-7549\",\"Tasks\":[30,4,13,64,5,27,6,20,20,73,31,35,44,49,37,63,1,68,15,2]},{\"Username\":\"cmartinho2\",\"Email\":\"kgosse2@aol.com\",\"Phone\":\"958-128-8777\",\"Tasks\":[22,65,54,26,37,36,63]},{\"Username\":\"mdilucia3\",\"Email\":\"nangus3@about.com\",\"Phone\":\"973-545-0173\",\"Tasks\":[7,9,37,49,51,35,30,25,34,7,51]},{\"Username\":\"dnapton4\",\"Email\":\"asprey4@businessinsider.com\",\"Phone\":\"235-815-6395\",\"Tasks\":[35,75,26,12,57,11,63]},{\"Username\":\"ostollsteimer5\",\"Email\":\"htocher5@w3.org\",\"Phone\":\"316-449-9725\",\"Tasks\":[21,46,12,2,62,37,10,46,14,24,50,6,6,23]},{\"Username\":\"codaly6\",\"Email\":\"rhaglington6@hibu.com\",\"Phone\":\"305-464-1823\",\"Tasks\":[16,5,4,29,39,16,13,21,75,4]},{\"Username\":\"qfilkin7\",\"Email\":\"mhanselmann7@soundcloud.com\",\"Phone\":\"678-790-6495\",\"Tasks\":[15,18,34,58,73]},{\"Username\":\"rkitchingham8\",\"Email\":\"rlibreros8@sina.com.cn\",\"Phone\":\"896-540-7234\",\"Tasks\":[65,72,46,33,46,60,44,22]},{\"Username\":\"kmurrow9\",\"Email\":\"jgauvain9@people.com.cn\",\"Phone\":\"273-877-4082\",\"Tasks\":[21,71,56,24,21,62,37,38,57,53,67,29,35,19,69,35]},{\"Username\":\"mbarendtsena\",\"Email\":\"srountreea@google.it\",\"Phone\":\"567-348-2054\",\"Tasks\":[53,20,74,73,48,13,72,16,65,48,42,15,54,7,47]},{\"Username\":\"pnicolsonb\",\"Email\":\"dbeinckenb@earthlink.net\",\"Phone\":\"349-526-0430\",\"Tasks\":[46,45,75,44,42,9,64,56,70,56,34,74,1,33,72]},{\"Username\":\"olathleiffurec\",\"Email\":\"aspohrmannc@ezinearticles.com\",\"Phone\":\"782-166-4860\",\"Tasks\":[10,54,17,29,31,38,2,74,28,66,21,14,65,59,17,41,22,16]},{\"Username\":\"btorrejond\",\"Email\":\"nskerrittd@mashable.com\",\"Phone\":\"405-758-1588\",\"Tasks\":[63,65,15,67,28,59,65,69,53,30,55,56,9,67,37]},{\"Username\":\"mdivere\",\"Email\":\"shawarde@squarespace.com\",\"Phone\":\"888-104-4239\",\"Tasks\":[3,62,41,17,8]},{\"Username\":\"tridolfif\",\"Email\":\"eterrazzof@slideshare.net\",\"Phone\":\"262-575-8196\",\"Tasks\":[74,74,62,10,31,28,31,19,35,19,40,36,56,46,48,71]},{\"Username\":\"sthynneg\",\"Email\":\"rderocheg@techcrunch.com\",\"Phone\":\"194-364-8234\",\"Tasks\":[55,26,65,37,72,13,6,68,26,13,67]},{\"Username\":\"gkeemsh\",\"Email\":\"qcaulfieldh@dropbox.com\",\"Phone\":\"904-722-6709\",\"Tasks\":[29,72,68,19,3]},{\"Username\":\"ehawtoni\",\"Email\":\"prousselli@artisteer.com\",\"Phone\":\"113-391-9570\",\"Tasks\":[54,10,21,54,75]},{\"Username\":\"nbremenj\",\"Email\":\"gpourvoieurj@guardian.co.uk\",\"Phone\":\"324-833-2254\",\"Tasks\":[23,16,46,37,66,31,7,37,2,26,39,32,46,55,6,8,24]},{\"Username\":\"emccarryk\",\"Email\":\"wgrindrodk@mysql.com\",\"Phone\":\"959-210-4953\",\"Tasks\":[74,22,62,8,56,31,70,42]},{\"Username\":\"mkyncll\",\"Email\":\"jbarbierl@imdb.com\",\"Phone\":\"184-104-0626\",\"Tasks\":[32,63,42,15,42,35,22,64,31]},{\"Username\":\"mobispom\",\"Email\":\"smcdonoghm@jimdo.com\",\"Phone\":\"589-982-5040\",\"Tasks\":[71,3,9,47,6,75,23,65,35,38]},{\"Username\":\"lmarquissn\",\"Email\":\"graglessn@berkeley.edu\",\"Phone\":\"239-404-5576\",\"Tasks\":[51,49,10,8,26,8,58,15,74,36,33]},{\"Username\":\"ocoolsono\",\"Email\":\"dchadbourneo@com.com\",\"Phone\":\"105-464-2474\",\"Tasks\":[34,56,43,4,60,7,14]},{\"Username\":\"jbehlingp\",\"Email\":\"crogetp@imdb.com\",\"Phone\":\"470-834-7902\",\"Tasks\":[51,44,67,39,30,49,22,38,25,34,48,49,67,18,16,12]},{\"Username\":\"pclaffeyq\",\"Email\":\"cussherq@tumblr.com\",\"Phone\":\"225-876-9150\",\"Tasks\":[25,61,17,70,4,57,33,12,59,33,19,74,37,12,29,16,69,42]},{\"Username\":\"bbetoniar\",\"Email\":\"crosternr@bloglines.com\",\"Phone\":\"512-373-2485\",\"Tasks\":[33,19,50,41,42,59,41,50,5,9]},{\"Username\":\"hgreenhaughs\",\"Email\":\"ebirkbys@bandcamp.com\",\"Phone\":\"622-909-9951\",\"Tasks\":[24,9,45,48,67,44]},{\"Username\":\"ymarcumt\",\"Email\":\"pbottelstonet@google.co.uk\",\"Phone\":\"477-171-6520\",\"Tasks\":[31,20,50,71,46,28,22,57,64,1,18,70,15,28,58,10,39,23,23]}]";

        var actualOutput =
            TeisterMask.DataProcessor.Deserializer.ImportEmployees(context, inputJson).TrimEnd();

        var expectedOutput =
            "Invalid data!\r\nInvalid data!\r\nSuccessfully imported employee - jstanett0 with 5 tasks.\r\nInvalid data!\r\nInvalid data!\r\nInvalid data!\r\nInvalid data!\r\nSuccessfully imported employee - mmcellen1 with 15 tasks.\r\nInvalid data!\r\nInvalid data!\r\nInvalid data!\r\nSuccessfully imported employee - cmartinho2 with 4 tasks.\r\nSuccessfully imported employee - mdilucia3 with 9 tasks.\r\nInvalid data!\r\nInvalid data!\r\nInvalid data!\r\nSuccessfully imported employee - dnapton4 with 4 tasks.\r\nInvalid data!\r\nSuccessfully imported employee - ostollsteimer5 with 11 tasks.\r\nInvalid data!\r\nSuccessfully imported employee - codaly6 with 7 tasks.\r\nInvalid data!\r\nInvalid data!\r\nSuccessfully imported employee - qfilkin7 with 3 tasks.\r\nInvalid data!\r\nInvalid data!\r\nInvalid data!\r\nSuccessfully imported employee - rkitchingham8 with 4 tasks.\r\nInvalid data!\r\nInvalid data!\r\nInvalid data!\r\nInvalid data!\r\nInvalid data!\r\nInvalid data!\r\nInvalid data!\r\nSuccessfully imported employee - kmurrow9 with 7 tasks.\r\nInvalid data!\r\nInvalid data!\r\nInvalid data!\r\nInvalid data!\r\nInvalid data!\r\nInvalid data!\r\nSuccessfully imported employee - mbarendtsena with 8 tasks.\r\nInvalid data!\r\nInvalid data!\r\nInvalid data!\r\nInvalid data!\r\nInvalid data!\r\nInvalid data!\r\nSuccessfully imported employee - pnicolsonb with 8 tasks.\r\nInvalid data!\r\nInvalid data!\r\nInvalid data!\r\nInvalid data!\r\nInvalid data!\r\nSuccessfully imported employee - olathleiffurec with 12 tasks.\r\nInvalid data!\r\nInvalid data!\r\nInvalid data!\r\nInvalid data!\r\nInvalid data!\r\nInvalid data!\r\nInvalid data!\r\nInvalid data!\r\nSuccessfully imported employee - btorrejond with 5 tasks.\r\nInvalid data!\r\nSuccessfully imported employee - mdivere with 4 tasks.\r\nInvalid data!\r\nInvalid data!\r\nInvalid data!\r\nInvalid data!\r\nSuccessfully imported employee - tridolfif with 9 tasks.\r\nInvalid data!\r\nInvalid data!\r\nInvalid data!\r\nInvalid data!\r\nInvalid data!\r\nSuccessfully imported employee - sthynneg with 4 tasks.\r\nInvalid data!\r\nInvalid data!\r\nSuccessfully imported employee - gkeemsh with 3 tasks.\r\nInvalid data!\r\nInvalid data!\r\nSuccessfully imported employee - ehawtoni with 2 tasks.\r\nInvalid data!\r\nInvalid data!\r\nSuccessfully imported employee - nbremenj with 13 tasks.\r\nInvalid data!\r\nInvalid data!\r\nInvalid data!\r\nInvalid data!\r\nSuccessfully imported employee - emccarryk with 4 tasks.\r\nInvalid data!\r\nInvalid data!\r\nSuccessfully imported employee - mkyncll with 6 tasks.\r\nInvalid data!\r\nInvalid data!\r\nInvalid data!\r\nSuccessfully imported employee - mobispom with 7 tasks.\r\nInvalid data!\r\nInvalid data!\r\nSuccessfully imported employee - lmarquissn with 8 tasks.\r\nInvalid data!\r\nInvalid data!\r\nSuccessfully imported employee - ocoolsono with 5 tasks.\r\nInvalid data!\r\nSuccessfully imported employee - jbehlingp with 13 tasks.\r\nInvalid data!\r\nInvalid data!\r\nInvalid data!\r\nInvalid data!\r\nInvalid data!\r\nInvalid data!\r\nSuccessfully imported employee - pclaffeyq with 10 tasks.\r\nInvalid data!\r\nSuccessfully imported employee - bbetoniar with 7 tasks.\r\nInvalid data!\r\nSuccessfully imported employee - hgreenhaughs with 5 tasks.\r\nInvalid data!\r\nInvalid data!\r\nInvalid data!\r\nInvalid data!\r\nInvalid data!\r\nSuccessfully imported employee - ymarcumt with 12 tasks.";

        var assertContext = this.serviceProvider.GetService<TeisterMaskContext>();

        const int expectedEmployeesCount = 30;
        var actualEmployeesCount = assertContext.Employees.Count();

        const int expectedEmployeesTaskCount = 214;
        var actualEmployeesTaskCount = assertContext.EmployeesTasks.Count();

        Assert.That(actualEmployeesTaskCount, Is.EqualTo(expectedEmployeesTaskCount),
            $"Inserted {nameof(context.EmployeesTasks)} count is incorrect!");

        Assert.That(actualEmployeesCount, Is.EqualTo(expectedEmployeesCount),
            $"Inserted {nameof(context.Employees)} count is incorrect!");

        Assert.That(actualOutput, Is.EqualTo(expectedOutput).NoClip,
            $"{nameof(TeisterMask.DataProcessor.Deserializer.ImportEmployees)} output is incorrect!");
    }

    private static void SeedDatabase(TeisterMaskContext context)
    {
        var datasetsJson =
            "{\"Project\":[{\"Id\":1,\"Name\":\"Seadrill\",\"OpenDate\":\"2018-01-25T00:01:00\",\"DueDate\":\"2019-01-16T00:08:00\"},{\"Id\":2,\"Name\":\"First Trust Senior Loan Fund ETF\",\"OpenDate\":\"2018-01-02T00:04:00\",\"DueDate\":\"2019-01-21T00:01:00\"},{\"Id\":3,\"Name\":\"New Oriental\",\"OpenDate\":\"2018-01-27T00:01:00\",\"DueDate\":\"2019-01-25T00:07:00\"},{\"Id\":4,\"Name\":\"Two River Bancorp\",\"OpenDate\":\"2018-01-04T00:04:00\",\"DueDate\":\"2019-01-26T00:07:00\"},{\"Id\":5,\"Name\":\"Perceptron, Inc.\",\"OpenDate\":\"2018-01-20T00:02:00\",\"DueDate\":\"2018-01-15T00:12:00\"},{\"Id\":6,\"Name\":\"National Beverage Corp.\",\"OpenDate\":\"2018-01-16T00:03:00\",\"DueDate\":\"2019-01-28T00:03:00\"},{\"Id\":7,\"Name\":\"China Lending Corporation\",\"OpenDate\":\"2018-01-02T00:02:00\",\"DueDate\":null},{\"Id\":8,\"Name\":\"American National Bankshares, Inc.\",\"OpenDate\":\"2018-01-14T00:08:00\",\"DueDate\":\"2019-01-11T00:02:00\"},{\"Id\":9,\"Name\":\"Sumitomo Mitsui\",\"OpenDate\":\"2018-01-04T00:01:00\",\"DueDate\":\"2018-01-28T00:11:00\"},{\"Id\":10,\"Name\":\"Tekla Life\",\"OpenDate\":\"2018-01-11T00:10:00\",\"DueDate\":\"2018-01-08T00:12:00\"},{\"Id\":11,\"Name\":\"Public Storage\",\"OpenDate\":\"2018-01-02T00:07:00\",\"DueDate\":null},{\"Id\":12,\"Name\":\"SS S.A.\",\"OpenDate\":\"2018-01-19T00:09:00\",\"DueDate\":\"2019-01-21T00:04:00\"},{\"Id\":13,\"Name\":\"Key Energy Services, Inc.\",\"OpenDate\":\"2018-01-16T00:08:00\",\"DueDate\":null},{\"Id\":14,\"Name\":\"Alamos Gold Inc.\",\"OpenDate\":\"2018-01-01T00:01:00\",\"DueDate\":null},{\"Id\":15,\"Name\":\"Atlas Air\",\"OpenDate\":\"2018-01-23T00:10:00\",\"DueDate\":\"2019-01-12T00:01:00\"},{\"Id\":16,\"Name\":\"U.S. Bancorp\",\"OpenDate\":\"2018-01-22T00:01:00\",\"DueDate\":\"2019-01-14T00:07:00\"},{\"Id\":17,\"Name\":\"Medley Capital Corporation\",\"OpenDate\":\"2018-01-14T00:02:00\",\"DueDate\":\"2018-01-25T00:12:00\"},{\"Id\":18,\"Name\":\"Magna Inc.\",\"OpenDate\":\"2018-01-28T00:06:00\",\"DueDate\":\"2019-01-03T00:02:00\"},{\"Id\":19,\"Name\":\"Caleres, Inc.\",\"OpenDate\":\"2018-01-06T00:07:00\",\"DueDate\":null},{\"Id\":20,\"Name\":\"Cellectis S.A.\",\"OpenDate\":\"2018-01-18T00:03:00\",\"DueDate\":\"2019-01-15T00:02:00\"},{\"Id\":21,\"Name\":\"TearLab Corporation\",\"OpenDate\":\"2018-01-08T00:03:00\",\"DueDate\":\"2019-01-05T00:05:00\"},{\"Id\":22,\"Name\":\"SCYNEXIS, Inc.\",\"OpenDate\":\"2018-01-18T00:07:00\",\"DueDate\":\"2019-01-26T00:07:00\"},{\"Id\":23,\"Name\":\"America Corporation\",\"OpenDate\":\"2018-01-16T00:09:00\",\"DueDate\":\"2019-01-02T00:03:00\"},{\"Id\":24,\"Name\":\"America\",\"OpenDate\":\"2018-01-19T00:06:00\",\"DueDate\":\"2019-01-26T00:10:00\"},{\"Id\":25,\"Name\":\"Hyster-Yale\",\"OpenDate\":\"2018-01-05T00:02:00\",\"DueDate\":null},{\"Id\":26,\"Name\":\"Five Star\",\"OpenDate\":\"2018-01-31T00:10:00\",\"DueDate\":\"2019-01-16T00:06:00\"},{\"Id\":27,\"Name\":\"Vical\",\"OpenDate\":\"2018-01-19T00:08:00\",\"DueDate\":\"2019-01-14T00:02:00\"},{\"Id\":28,\"Name\":\"PPL Corporation\",\"OpenDate\":\"2018-01-30T00:09:00\",\"DueDate\":\"2019-01-03T00:03:00\"},{\"Id\":29,\"Name\":\"Land System\",\"OpenDate\":\"2018-01-16T00:03:00\",\"DueDate\":null},{\"Id\":30,\"Name\":\"Cypress Energy\",\"OpenDate\":\"2018-01-22T00:07:00\",\"DueDate\":\"2019-01-13T00:04:00\"},{\"Id\":31,\"Name\":\"Boston Private Financial Holdings, Inc.\",\"OpenDate\":\"2018-01-06T00:03:00\",\"DueDate\":\"2019-01-26T00:02:00\"},{\"Id\":32,\"Name\":\"TESARO, Inc.\",\"OpenDate\":\"2018-01-20T00:06:00\",\"DueDate\":\"2019-01-13T00:05:00\"},{\"Id\":33,\"Name\":\"Net 1 UEPS Technologies, Inc.\",\"OpenDate\":\"2018-01-20T00:01:00\",\"DueDate\":\"2019-01-02T00:04:00\"},{\"Id\":34,\"Name\":\"L Brands, Inc.\",\"OpenDate\":\"2018-01-24T00:10:00\",\"DueDate\":\"2019-01-14T00:03:00\"},{\"Id\":35,\"Name\":\"Blackrock MuniHoldings\",\"OpenDate\":\"2018-01-09T00:06:00\",\"DueDate\":\"2019-01-13T00:03:00\"},{\"Id\":36,\"Name\":\"Postman\",\"OpenDate\":\"2018-01-01T00:11:00\",\"DueDate\":\"2019-01-10T00:06:00\"},{\"Id\":37,\"Name\":\"VivoPower PLC\",\"OpenDate\":\"2018-01-02T00:01:00\",\"DueDate\":\"2019-01-30T00:08:00\"},{\"Id\":38,\"Name\":\"Arbor Trust\",\"OpenDate\":\"2018-01-01T00:09:00\",\"DueDate\":\"2019-01-05T00:09:00\"},{\"Id\":39,\"Name\":\"Greenbrier Companies\",\"OpenDate\":\"2018-01-02T00:02:00\",\"DueDate\":\"2019-01-23T00:05:00\"},{\"Id\":40,\"Name\":\"BBVA Banco Frances S.A.\",\"OpenDate\":\"2018-01-04T00:01:00\",\"DueDate\":\"2018-01-11T00:12:00\"},{\"Id\":41,\"Name\":\"First22\",\"OpenDate\":\"2018-01-22T00:05:00\",\"DueDate\":\"2019-01-23T00:01:00\"},{\"Id\":42,\"Name\":\"Brandywine\",\"OpenDate\":\"2018-01-15T00:10:00\",\"DueDate\":\"2019-01-21T00:07:00\"},{\"Id\":43,\"Name\":\"Interpace 22.\",\"OpenDate\":\"2018-01-20T00:10:00\",\"DueDate\":\"2019-01-19T00:01:00\"},{\"Id\":44,\"Name\":\"USANA Health Sciences, Inc.\",\"OpenDate\":\"2018-01-28T00:05:00\",\"DueDate\":\"2019-01-04T00:11:00\"}],\"Task\":[{\"Id\":1,\"Name\":\"Broadleaf\",\"OpenDate\":\"2018-03-21T00:00:00\",\"DueDate\":\"2018-12-18T00:00:00\",\"ExecutionType\":0,\"LabelType\":2,\"ProjectId\":25},{\"Id\":2,\"Name\":\"Yellow Wildrye\",\"OpenDate\":\"2018-07-26T00:00:00\",\"DueDate\":\"2018-12-12T00:00:00\",\"ExecutionType\":1,\"LabelType\":0,\"ProjectId\":22},{\"Id\":3,\"Name\":\"Cyrtandra\",\"OpenDate\":\"2018-08-04T00:00:00\",\"DueDate\":\"2018-11-14T00:00:00\",\"ExecutionType\":2,\"LabelType\":0,\"ProjectId\":2},{\"Id\":4,\"Name\":\"Burchell's Clover\",\"OpenDate\":\"2018-06-17T00:00:00\",\"DueDate\":\"2018-12-03T00:00:00\",\"ExecutionType\":2,\"LabelType\":4,\"ProjectId\":3},{\"Id\":5,\"Name\":\"Spreading Sandwort\",\"OpenDate\":\"2018-02-19T00:00:00\",\"DueDate\":\"2018-11-20T00:00:00\",\"ExecutionType\":2,\"LabelType\":4,\"ProjectId\":7},{\"Id\":6,\"Name\":\"Loeskypnum Moss\",\"OpenDate\":\"2018-07-28T00:00:00\",\"DueDate\":\"2018-11-20T00:00:00\",\"ExecutionType\":3,\"LabelType\":2,\"ProjectId\":8},{\"Id\":7,\"Name\":\"Fir Clubmoss\",\"OpenDate\":\"2018-09-15T00:00:00\",\"DueDate\":\"2019-07-08T00:00:00\",\"ExecutionType\":3,\"LabelType\":3,\"ProjectId\":11},{\"Id\":8,\"Name\":\"Oregon Figwort\",\"OpenDate\":\"2018-05-12T00:00:00\",\"DueDate\":\"2019-09-06T00:00:00\",\"ExecutionType\":3,\"LabelType\":1,\"ProjectId\":11},{\"Id\":9,\"Name\":\"Bullfrog Mountain Pea\",\"OpenDate\":\"2018-01-18T00:00:00\",\"DueDate\":\"2018-12-10T00:00:00\",\"ExecutionType\":3,\"LabelType\":2,\"ProjectId\":11},{\"Id\":10,\"Name\":\"Indian Canyon Fleabane\",\"OpenDate\":\"2018-03-25T00:00:00\",\"DueDate\":\"2019-01-05T00:00:00\",\"ExecutionType\":3,\"LabelType\":1,\"ProjectId\":11},{\"Id\":11,\"Name\":\"Nodding Sage\",\"OpenDate\":\"2018-04-02T00:00:00\",\"DueDate\":\"2019-08-19T00:00:00\",\"ExecutionType\":2,\"LabelType\":3,\"ProjectId\":11},{\"Id\":12,\"Name\":\"American Star-thistle\",\"OpenDate\":\"2018-09-21T00:00:00\",\"DueDate\":\"2018-11-29T00:00:00\",\"ExecutionType\":0,\"LabelType\":1,\"ProjectId\":22},{\"Id\":13,\"Name\":\"Arctic Raspberry\",\"OpenDate\":\"2018-06-23T00:00:00\",\"DueDate\":\"2019-01-29T00:00:00\",\"ExecutionType\":0,\"LabelType\":3,\"ProjectId\":11},{\"Id\":14,\"Name\":\"Uvero De Monte\",\"OpenDate\":\"2018-06-19T00:00:00\",\"DueDate\":\"2019-06-14T00:00:00\",\"ExecutionType\":2,\"LabelType\":0,\"ProjectId\":13},{\"Id\":15,\"Name\":\"Roundleaf Leather-root\",\"OpenDate\":\"2018-08-28T00:00:00\",\"DueDate\":\"2019-04-11T00:00:00\",\"ExecutionType\":3,\"LabelType\":0,\"ProjectId\":13},{\"Id\":16,\"Name\":\"Felt Lichen\",\"OpenDate\":\"2018-06-14T00:00:00\",\"DueDate\":\"2019-09-06T00:00:00\",\"ExecutionType\":2,\"LabelType\":3,\"ProjectId\":13},{\"Id\":17,\"Name\":\"Stickystem Penstemon\",\"OpenDate\":\"2018-05-10T00:00:00\",\"DueDate\":\"2019-04-10T00:00:00\",\"ExecutionType\":2,\"LabelType\":1,\"ProjectId\":13},{\"Id\":18,\"Name\":\"Columbian\",\"OpenDate\":\"2018-10-24T00:00:00\",\"DueDate\":\"2019-10-20T00:00:00\",\"ExecutionType\":2,\"LabelType\":4,\"ProjectId\":13},{\"Id\":19,\"Name\":\"Arizona Hedgehog Cactus\",\"OpenDate\":\"2018-05-06T00:00:00\",\"DueDate\":\"2018-12-17T00:00:00\",\"ExecutionType\":3,\"LabelType\":0,\"ProjectId\":13},{\"Id\":20,\"Name\":\"Comfortroot\",\"OpenDate\":\"2018-07-14T00:00:00\",\"DueDate\":\"2019-06-10T00:00:00\",\"ExecutionType\":3,\"LabelType\":0,\"ProjectId\":14},{\"Id\":21,\"Name\":\"Bicolored Spleenwort\",\"OpenDate\":\"2018-05-24T00:00:00\",\"DueDate\":\"2019-07-02T00:00:00\",\"ExecutionType\":2,\"LabelType\":1,\"ProjectId\":14},{\"Id\":22,\"Name\":\"Munz's Buckwheat\",\"OpenDate\":\"2018-07-18T00:00:00\",\"DueDate\":\"2018-12-18T00:00:00\",\"ExecutionType\":0,\"LabelType\":1,\"ProjectId\":16},{\"Id\":23,\"Name\":\"Plains Flax\",\"OpenDate\":\"2018-02-07T00:00:00\",\"DueDate\":\"2018-11-22T00:00:00\",\"ExecutionType\":3,\"LabelType\":0,\"ProjectId\":19},{\"Id\":24,\"Name\":\"Clustered Sawwort\",\"OpenDate\":\"2018-07-21T00:00:00\",\"DueDate\":\"2018-12-09T00:00:00\",\"ExecutionType\":1,\"LabelType\":0,\"ProjectId\":11},{\"Id\":25,\"Name\":\"Scarlet Globemallow\",\"OpenDate\":\"2018-06-08T00:00:00\",\"DueDate\":\"2018-12-29T00:00:00\",\"ExecutionType\":3,\"LabelType\":1,\"ProjectId\":43},{\"Id\":26,\"Name\":\"Little Bluestem\",\"OpenDate\":\"2018-07-04T00:00:00\",\"DueDate\":\"2018-12-02T00:00:00\",\"ExecutionType\":2,\"LabelType\":0,\"ProjectId\":42},{\"Id\":27,\"Name\":\"Clammy\",\"OpenDate\":\"2018-04-30T00:00:00\",\"DueDate\":\"2019-01-03T00:00:00\",\"ExecutionType\":1,\"LabelType\":2,\"ProjectId\":41},{\"Id\":28,\"Name\":\"Cornflag\",\"OpenDate\":\"2018-09-27T00:00:00\",\"DueDate\":\"2019-09-25T00:00:00\",\"ExecutionType\":1,\"LabelType\":1,\"ProjectId\":25},{\"Id\":29,\"Name\":\"Debeque\",\"OpenDate\":\"2018-10-17T00:00:00\",\"DueDate\":\"2019-01-25T00:00:00\",\"ExecutionType\":3,\"LabelType\":2,\"ProjectId\":25},{\"Id\":30,\"Name\":\"Bryum\",\"OpenDate\":\"2018-11-02T00:00:00\",\"DueDate\":\"2019-01-19T00:00:00\",\"ExecutionType\":0,\"LabelType\":3,\"ProjectId\":25},{\"Id\":31,\"Name\":\"Pacific\",\"OpenDate\":\"2018-05-14T00:00:00\",\"DueDate\":\"2019-07-05T00:00:00\",\"ExecutionType\":3,\"LabelType\":0,\"ProjectId\":25},{\"Id\":32,\"Name\":\"Guadalupe\",\"OpenDate\":\"2018-09-29T00:00:00\",\"DueDate\":\"2018-12-23T00:00:00\",\"ExecutionType\":0,\"LabelType\":2,\"ProjectId\":25},{\"Id\":33,\"Name\":\"Crandall\",\"OpenDate\":\"2018-03-12T00:00:00\",\"DueDate\":\"2019-02-21T00:00:00\",\"ExecutionType\":2,\"LabelType\":0,\"ProjectId\":25},{\"Id\":34,\"Name\":\"Longbract Pohlia Moss\",\"OpenDate\":\"2018-09-13T00:00:00\",\"DueDate\":\"2019-03-29T00:00:00\",\"ExecutionType\":2,\"LabelType\":3,\"ProjectId\":25},{\"Id\":35,\"Name\":\"Guadeloupe\",\"OpenDate\":\"2018-02-14T00:00:00\",\"DueDate\":\"2019-02-09T00:00:00\",\"ExecutionType\":1,\"LabelType\":2,\"ProjectId\":25},{\"Id\":36,\"Name\":\"Meyen's Sedge\",\"OpenDate\":\"2018-10-09T00:00:00\",\"DueDate\":\"2019-05-05T00:00:00\",\"ExecutionType\":0,\"LabelType\":3,\"ProjectId\":25},{\"Id\":37,\"Name\":\"Back\",\"OpenDate\":\"2018-06-07T00:00:00\",\"DueDate\":\"2018-12-30T00:00:00\",\"ExecutionType\":2,\"LabelType\":4,\"ProjectId\":28},{\"Id\":38,\"Name\":\"Cypress Panicgrass\",\"OpenDate\":\"2018-10-19T00:00:00\",\"DueDate\":\"2018-11-17T00:00:00\",\"ExecutionType\":2,\"LabelType\":3,\"ProjectId\":42},{\"Id\":39,\"Name\":\"White\",\"OpenDate\":\"2018-10-04T00:00:00\",\"DueDate\":\"2019-04-21T00:00:00\",\"ExecutionType\":1,\"LabelType\":4,\"ProjectId\":29},{\"Id\":40,\"Name\":\"Paradox\",\"OpenDate\":\"2018-05-11T00:00:00\",\"DueDate\":\"2019-05-15T00:00:00\",\"ExecutionType\":2,\"LabelType\":4,\"ProjectId\":29},{\"Id\":41,\"Name\":\"Annual\",\"OpenDate\":\"2018-05-10T00:00:00\",\"DueDate\":\"2019-06-07T00:00:00\",\"ExecutionType\":0,\"LabelType\":1,\"ProjectId\":29},{\"Id\":42,\"Name\":\"Nipa Palm\",\"OpenDate\":\"2018-10-14T00:00:00\",\"DueDate\":\"2019-09-19T00:00:00\",\"ExecutionType\":2,\"LabelType\":0,\"ProjectId\":29},{\"Id\":43,\"Name\":\"Charleston Mousetail\",\"OpenDate\":\"2018-08-10T00:00:00\",\"DueDate\":\"2019-07-07T00:00:00\",\"ExecutionType\":0,\"LabelType\":4,\"ProjectId\":29},{\"Id\":44,\"Name\":\"Northland\",\"OpenDate\":\"2018-07-31T00:00:00\",\"DueDate\":\"2019-04-06T00:00:00\",\"ExecutionType\":1,\"LabelType\":1,\"ProjectId\":29},{\"Id\":45,\"Name\":\"Sturdy Bulrush\",\"OpenDate\":\"2018-10-25T00:00:00\",\"DueDate\":\"2018-11-20T00:00:00\",\"ExecutionType\":1,\"LabelType\":3,\"ProjectId\":31},{\"Id\":46,\"Name\":\"Spotted Phacelia\",\"OpenDate\":\"2018-06-14T00:00:00\",\"DueDate\":\"2018-12-02T00:00:00\",\"ExecutionType\":1,\"LabelType\":2,\"ProjectId\":33},{\"Id\":47,\"Name\":\"Ashy Sandmat\",\"OpenDate\":\"2018-04-06T00:00:00\",\"DueDate\":\"2018-12-18T00:00:00\",\"ExecutionType\":3,\"LabelType\":2,\"ProjectId\":34},{\"Id\":48,\"Name\":\"Wetslope Buttercup\",\"OpenDate\":\"2018-03-07T00:00:00\",\"DueDate\":\"2018-11-21T00:00:00\",\"ExecutionType\":2,\"LabelType\":3,\"ProjectId\":36},{\"Id\":49,\"Name\":\"Calophyllum\",\"OpenDate\":\"2018-10-09T00:00:00\",\"DueDate\":\"2018-11-15T00:00:00\",\"ExecutionType\":2,\"LabelType\":1,\"ProjectId\":39},{\"Id\":50,\"Name\":\"Blueeyed\",\"OpenDate\":\"2018-10-22T00:00:00\",\"DueDate\":\"2019-06-25T00:00:00\",\"ExecutionType\":1,\"LabelType\":2,\"ProjectId\":29},{\"Id\":51,\"Name\":\"Globe Ball Lichen\",\"OpenDate\":\"2018-07-23T00:00:00\",\"DueDate\":\"2018-11-18T00:00:00\",\"ExecutionType\":1,\"LabelType\":1,\"ProjectId\":43}]}";
        var datasets = JsonConvert.DeserializeObject<Dictionary<string, IEnumerable<JObject>>>(datasetsJson);

        foreach (var dataset in datasets)
        {
            var entityType = GetType(dataset.Key);
            var entities = dataset.Value
                .Select(j => j.ToObject(entityType))
                .ToArray();

            context.AddRange(entities);
        }

        context.SaveChanges();
    }

    private static Type GetType(string modelName)
    {
        var modelType = CurrentAssembly
            .GetTypes()
            .FirstOrDefault(t => t.Name == modelName);

        Assert.IsNotNull(modelType, $"{modelName} model not found!");

        return modelType;
    }

    private static IServiceProvider ConfigureServices<TContext>(string databaseName)
        where TContext : DbContext
    {
        var services = ConfigureDbContext<TContext>(databaseName);

        var context = services.GetService<TContext>();

        //try
        //{
        //    context.Model.GetEntityTypes();
       // }
        //catch (InvalidOperationException ex) when (ex.Source == "Microsoft.EntityFrameworkCore.Proxies")
        //{
            services = ConfigureDbContext<TContext>(databaseName, useLazyLoading: true);
        //}

        return services;
    }

    private static IServiceProvider ConfigureDbContext<TContext>(string databaseName, bool useLazyLoading = false)
        where TContext : DbContext
    {
        var services = new ServiceCollection()
           .AddDbContext<TContext>(t => t
           .UseInMemoryDatabase(Guid.NewGuid().ToString())
           );

        var serviceProvider = services.BuildServiceProvider();
        return serviceProvider;
    }
}