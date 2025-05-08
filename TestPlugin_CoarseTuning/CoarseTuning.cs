using ChartDirector;
using CSharpMtGrouper;
using CSharpMtGrouper.classes;
using CSharpMtGrouper.modules;
using General;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestPlugin_CoarseTuning
{
    public class CoarseTuning
    {
        //instance variables
        private double[] mirr1Vals;
        private double[] mirr2Vals;
        private double[] wlVals;
        private List<clsMtMidline> midLines = new List<clsMtMidline>();
        private List<(double[], double[], double[])> wlGroups = new List<(double[], double[], double[])>();
        private List<clsLabeledItuChannelPoint> ituChannels = new List<clsLabeledItuChannelPoint>();
        clsMidlineFinder midlineAndItuFinder;
        private bool m_isReading3dData = true;
        private double[] _z;
        private double[] _x;
        private double[] _y;

        public bool IsReading3dData
        {
            get
            {
                return this.m_isReading3dData;
            }
            set
            {
                this.m_isReading3dData = value;
            }
        }
        public double[] X
        {
            get
            {
                return this._x;
            }
            set
            {
                this._x = value;
            }
        }

        public double[] Y
        {
            get
            {
                return this._y;
            }
            set
            {
                this._y = value;
            }
        }

        public double[] Z
        {
            get
            {
                return this._z;
            }
            set
            {
                this._z = value;
            }
        }

        public void ReadRawDataFromFile(string fileName)
        {
            List<double> xVals = new List<double>();
            List<double> yVals = new List<double>();
            List<double> zVals = new List<double>();
            try
            {
                using (StreamReader reader = new StreamReader(fileName))
                {
                    while (!reader.EndOfStream)
                    {
                        string currentLine = reader.ReadLine();
                        bool readingRawData = false; ;
                        bool flag = readingRawData;
                        if (flag)
                        {
                            string[] lineParts = currentLine.Split(new char[]
                            {
                                ','
                            });
                            double currentXVal = Conversions.ToDouble(lineParts[0]);
                            double currentYVal = Conversions.ToDouble(lineParts[1]);
                            bool isReading3dData = this.IsReading3dData;
                            double currentZVal = 0.0;
                            if (isReading3dData)
                            {
                                bool flag2 = !double.TryParse(lineParts[2], out currentZVal);
                                if (flag2)
                                {
                                    currentZVal = 0.0;
                                }
                            }
                            xVals.Add(currentXVal);
                            yVals.Add(currentYVal);
                            bool isReading3dData2 = this.IsReading3dData;
                            if (isReading3dData2)
                            {
                                zVals.Add(Math.Max(currentZVal, 0.0));
                            }
                        }
                        bool flag3 = !readingRawData;
                        if (flag3)
                        {
                            readingRawData = (currentLine.Equals("[RAW DATA]") | currentLine.Equals("[DATA]"));
                        }
                    }
                    this.X = xVals.ToArray();
                    this.Y = yVals.ToArray();
                    bool isReading3dData3 = this.IsReading3dData;
                    if (isReading3dData3)
                    {
                        this.Z = zVals.ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// Reads the mirror map wavelength file, and extracts the mirror1, mirror2, and waveelngth values
        /// </summary>
        public bool ReadMirrorMapWavelengthFileAndSetupItuHelper(string path = "")
        {
            try
            {
                string mirrorMapFileName = string.Empty; ;
                if (path == "")
                {
                    //COPY THIS FILE PATH FROM YOUR 
                    OpenFileDialog ofd = new OpenFileDialog()
                    {
                        InitialDirectory = AppDomain.CurrentDomain.BaseDirectory,
                    };
                    var dr = ofd.ShowDialog();
                    if (dr != DialogResult.OK)
                    {
                        return false;

                    }
                    mirrorMapFileName = ofd.FileName;
                }
                else
                {
                    mirrorMapFileName = path;
                }

                //this.ReadRawDataFromFile(mirrorMapFileName);
                //string mirrorMapFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DO123_TM124_T0326_MirrorMapping#Wavelength@34.64C_2024-04-02_10-48-50.csv");
                clsCsvHandler mirrorMapReader = new clsCsvHandler();
                //configure the reader to read 3d data, then trigger the read
                mirrorMapReader.IsReading3dData = true;
                mirrorMapReader.ReadRawDataFromFile(mirrorMapFileName);
                //extract the mirror1/2 and WL info
                mirr1Vals = mirrorMapReader.X;
                mirr2Vals = mirrorMapReader.Y;
                wlVals = mirrorMapReader.Z;
                //set up the midline and itu channel finder
                midlineAndItuFinder = new clsMidlineFinder(mirr1Vals, mirr2Vals, wlVals);
                midlineAndItuFinder.InitialGroupThreshold = 1;
                midlineAndItuFinder.FinalGroupThreshold = 0.5;


                return true;
                //midlineAndItuFinder.InitialGroupThreshold = 100;
                //midlineAndItuFinder.FinalGroupThreshold = -100;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }


        /// <summary>
        /// Takes the mirror mapping data (mirrors 1 and 2, and WL values), and the groups them
        /// </summary>
        /// <remarks>
        /// After reading in the wavelength data, this should be step 1 in the midline/itu channel finding process
        /// </remarks>
        public void GroupWavelegnthValues()
        {
            try
            {
                wlGroups = midlineAndItuFinder.GetWavelengthPlotGroups();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Gets the midline data from the wavelength groups gathered in step 1
        /// </summary>
        /// <remarks>
        /// This should be step 2 in the midline/itu channel finding process.
        /// </remarks>
        public void PopulateMidlines()
        {
            try
            {
                midLines = midlineAndItuFinder.GetAllMidlines(groups: wlGroups);
                //IMPORTANT: we must map an index to each midline so they can be properly saved in csv format
                modMidlineAndItuChannelInfo.BuildMidlineMap(midLines);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        /// <summary>
        /// Gets all itu channels.
        /// </summary>
        /// <remarks>
        /// This is the last step in computing the itu channels
        /// </remarks>
        public void GetAllItuChannels()
        {
            try
            {
                //get the list of target wavelengths from channels 1 to 96
                List<double> ituChannelWavelengths = clsMtGrouperFunctions.GetItuWavelengthRange(-19, 110);
                //The following lines is how the VB gui does the coarse tuning: there may be a more concise way to write this, but this sequence
                //has been tested, and works well for the time being
                var mirr1ItuData = clsMtGrouperFunctions.ItuGridAll(midLines, ituChannelWavelengths.ToArray(), modUniversalLaserControllerHelpers.MIRROR1);
                var mirr2ItuData = clsMtGrouperFunctions.ItuGridAll(midLines, ituChannelWavelengths.ToArray(), modUniversalLaserControllerHelpers.MIRROR2);
                double[] interpolatedMirr1Currents = mirr1ItuData.Item1;
                double[] interpolatedMirr2Currents = mirr2ItuData.Item1;
                int[] midlinePointers = mirr1ItuData.Item4;
                double[] targetChannelWavelengths = mirr1ItuData.Item2;

                //20241111 如果有异常, 不执行下面的代码
                if (interpolatedMirr1Currents.Length > 0 && interpolatedMirr2Currents.Length > 0 && midlinePointers.Length > 0 && targetChannelWavelengths.Length > 0)
                {
                    //command that generates the labeled points
                    ituChannels = midlineAndItuFinder.GetLabledItuPoints(-19, targetChannelWavelengths, interpolatedMirr1Currents, interpolatedMirr2Currents, midlinePointers);
                    AssociateItuChannelsWithMidlines();
                }
            }
            catch (Exception ex)
            {
                
                MessageBox.Show(ex.Message);
            }

        }

        /// <summary>
        /// Assocaites each found itu channel with its midline
        /// </summary>
        public void AssociateItuChannelsWithMidlines()
        {
            if (midLines != null && ituChannels != null)
            {
                foreach (var channel in ituChannels)
                {
                    modMidlineAndItuChannelInfo.g_indexToMidlineAssoc[channel.MidlineIndex].AddChannelAssociation(channel);
                }
            }
        }

        public void PlotWavelengthGroups(ref WinChartViewer Chart_Groups)
        {
            try
            {
                XYChart plotArea = modChartDirectorPlotHelpers.SetupPlotArea(ref Chart_Groups, "Mirror1[mA]", "Mirror2[mA]", 30, 30, 30, 30);
                //set axis bounds for the chart
                modChartDirectorPlotHelpers.SetXAxisLinearScale(ref plotArea, mirr1Vals.Min(), mirr2Vals.Max(), 10, 2);
                modChartDirectorPlotHelpers.SetYAxisLinearScale(ref plotArea, mirr2Vals.Min(), mirr2Vals.Max(), 10, 2);
                //for each wavelength group, plot mirror1 and mirror2 as a scatter plot, and add a contour layer for the wavelengths
                foreach (var group in wlGroups)
                {
                    double[] grpMirr1Vals = group.Item1;
                    double[] grpMirr2Vals = group.Item2;
                    double[] grpWlVals = group.Item3;
                    modChartDirectorPlotHelpers.AddScatterPlot(ref plotArea, grpMirr1Vals, grpMirr2Vals);
                    plotArea.addContourLayer(grpMirr1Vals, grpMirr2Vals, grpWlVals);
                }
                modChartDirectorPlotHelpers.UpdateChart(ref Chart_Groups, plotArea);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void PlotMidlines(ref WinChartViewer Chart_Midlines)
        {
            try
            {
                XYChart plotArea = modChartDirectorPlotHelpers.SetupPlotArea(ref Chart_Midlines, "Mirror1[mA]", "Mirror2[mA]", 30, 30, 30, 30);
                //set up the axis bounds
                modChartDirectorPlotHelpers.SetXAxisLinearScale(ref plotArea, mirr1Vals.Min(), mirr1Vals.Max(), 10, 2);
                modChartDirectorPlotHelpers.SetYAxisLinearScale(ref plotArea, mirr2Vals.Min(), mirr2Vals.Max(), 10, 2);
                //for each midline, plot it as a scatter plot
                foreach (var midline in midLines)
                {
                    modChartDirectorPlotHelpers.AddScatterPlot(ref plotArea, midline.Mirr1Vals.ToArray(), midline.Mirr2Vals.ToArray());
                }
                //add the mirror map wavelength data
                double[] filteredWlVals = General.modGlobals.ClampArrayValues(wlVals, 1510, 1580); //filter the data to get a good graph scale, "zero" points will mess up the scale
                ContourLayer wlLayer = plotArea.addContourLayer(mirr1Vals, mirr2Vals, filteredWlVals);
                modChartDirectorPlotHelpers.UpdateChart(ref Chart_Midlines, plotArea);
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void PlotLabeledItuChannels(ref WinChartViewer Chart_LabeledPoints)
        {
            try
            {
                //extract the mirror1 and mirror 2 from each labeled channel
                double[] ituChannelMirr1Values = ituChannels.Select(channel => channel.Mirr1Current).ToArray();
                double[] ituChannelMirr2Values = ituChannels.Select(channel => channel.Mirr2Current).ToArray();
                string[] ituLabels = ituChannels.Select(channel => channel.Label.ToString()).ToArray();
                XYChart plotArea = modChartDirectorPlotHelpers.SetupPlotArea(ref Chart_LabeledPoints, "Mirror1[mA]", "Mirror2[mA]", 30, 30, 30, 30);



                //set axis bounds
                modChartDirectorPlotHelpers.SetXAxisLinearScale(ref plotArea, mirr1Vals.Min(), mirr1Vals.Max(), 10, 2);
                modChartDirectorPlotHelpers.SetYAxisLinearScale(ref plotArea, mirr2Vals.Min(), mirr2Vals.Max(), 10, 2);
                //add scatter plot
                ScatterLayer ituScatter = plotArea.addScatterLayer(ituChannelMirr1Values, ituChannelMirr2Values);
                //add the labels to the plot
                ituScatter.addExtraField(ituLabels);
                ituScatter.setDataLabelFormat("{field0}");
                //add the wavelength data to the plot
                double[] filteredWlVals = modGlobals.ClampArrayValues(wlVals, 1510, 1580); //filter out any abnormal wl values to avoid a weird scale
                plotArea.addContourLayer(mirr1Vals, mirr2Vals, filteredWlVals);
                modChartDirectorPlotHelpers.UpdateChart(ref Chart_LabeledPoints, plotArea);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        //public void PlotLabeledItuChannels(ref WinChartViewer chart)
        //{
        //    try
        //    {
        //        //extract the mirror1 and mirror 2 from each labeled channel
        //        double[] ituChannelMirr1Values = ituChannels.Select(channel => channel.Mirr1Current).ToArray();
        //        double[] ituChannelMirr2Values = ituChannels.Select(channel => channel.Mirr2Current).ToArray();
        //        string[] ituLabels = ituChannels.Select(channel => channel.Label.ToString()).ToArray();
        //        XYChart plotArea = modChartDirectorPlotHelpers.SetupPlotArea(ref chart, "Mirror1[mA]", "Mirror2[mA]", 30, 30, 30, 30);



        //        //set axis bounds
        //        modChartDirectorPlotHelpers.SetXAxisLinearScale(ref plotArea, mirr1Vals.Min(), mirr1Vals.Max(), 10, 2);
        //        modChartDirectorPlotHelpers.SetYAxisLinearScale(ref plotArea, mirr2Vals.Min(), mirr2Vals.Max(), 10, 2);
        //        //add scatter plot
        //        ScatterLayer ituScatter = plotArea.addScatterLayer(ituChannelMirr1Values, ituChannelMirr2Values);
        //        //add the labels to the plot
        //        ituScatter.addExtraField(ituLabels);
        //        ituScatter.setDataLabelFormat("{field0}");
        //        //add the wavelength data to the plot
        //        double[] filteredWlVals = modGlobals.ClampArrayValues(wlVals, 1510, 1580); //filter out any abnormal wl values to avoid a weird scale
        //        plotArea.addContourLayer(mirr1Vals, mirr2Vals, filteredWlVals);
        //        modChartDirectorPlotHelpers.UpdateChart(ref chart, plotArea);
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //}
        public void SaveMidlineCsvFile()
        {
            try
            {
                clsMidlineCsvHandler midlineInfoWriter = new clsMidlineCsvHandler();
                var csvRows = BuildMidlineCsvRows();

                if (csvRows != null && csvRows.Count > 0)
                {
                    midlineInfoWriter.WriteMidlineDataToCsvV2(modFolderAndFileAndImageNaming.COARSE_TUNING_FOLDERNAME, modFolderAndFileAndImageNaming.COARSE_TUNING_FILENAME, false, "", csvRows, modFolderAndFileAndImageNaming.COARSE_TUNING_MIDLINE_INFO_NAME);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void SaveItuPointsCsvFile()
        {
            try
            {
                clsItuChannelCsvHandler ituChannelWriter = new clsItuChannelCsvHandler();
                ituChannelWriter.CoarseTuningMode = true; //set this to true so the proper column header is used in the csv file.
                List<List<string>> rowsToWrite = BuildItuChannelCsvRows();
                if (rowsToWrite != null && rowsToWrite.Count > 0)
                {
                    ituChannelWriter.WriteItuChannelDataToCsvV2(modFolderAndFileAndImageNaming.COARSE_TUNING_FOLDERNAME,
                        modFolderAndFileAndImageNaming.COARSE_TUNING_FILENAME, false, "",
                        rowsToWrite, modFolderAndFileAndImageNaming.COARSE_TUNING_CHANNEL_INFO_NAME);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Builds the itu channel CSV rows where 1 row corresponds to 1 channel
        /// </summary>
        /// <returns>List&lt;List&lt;System.String&gt;&gt;.</returns>
        public List<List<string>> BuildItuChannelCsvRows()
        {
            List<List<string>> csvRows = new List<List<string>>();
            if (ituChannels != null)
            {
                foreach (var channel in ituChannels)
                {
                    //populate all current source electrodes except the mirrors with the currents that were used during the test
                    //these are just examples
                    channel.GainCurrent = 120;
                    channel.Soa1Current = 50;
                    channel.Soa2Current = 50;
                    channel.LaserPhaseCurrent = 3;
                    channel.Phase1Current = 0;
                    channel.Phase2Current = 0;
                    channel.Mzm1VBias = -2.5;
                    channel.Mzm1VBias = -2.5;
                    channel.CoarseTuningMode = true;
                    csvRows.Add(channel.GetItuPointDataAsCsvRow());
                }
            }
            return csvRows;
        }

        /// <summary>
        /// Build a list of rows where each row represents a csv section for a single midline
        /// </summary>
        /// <returns></returns>
        public List<List<string>> BuildMidlineCsvRows()
        {
            List<List<string>> csvRows = new List<List<string>>();
            if (modMidlineAndItuChannelInfo.g_indexToMidlineAssoc == null)
            {
                return null;
            }
            foreach (KeyValuePair<int, clsMtMidline> kvp in modMidlineAndItuChannelInfo.g_indexToMidlineAssoc)
            {
                int midlineIndex = kvp.Key;
                clsMtMidline midline = kvp.Value;
                midline.MyIndex = midlineIndex; //VERY IMPORTANT to save the index of the midline in each row
                csvRows.Add(midline.GetMidlineInfoAsCsvRow());

            }
            return csvRows;
        }



        public void ClearChart(ref WinChartViewer Chart_Groups, ref WinChartViewer Chart_Midlines, ref WinChartViewer Chart_LabeledPoints)
        {
            modChartDirectorPlotHelpers.ClearPlot(ref Chart_Groups);
            modChartDirectorPlotHelpers.ClearPlot(ref Chart_Midlines);
            modChartDirectorPlotHelpers.ClearPlot(ref Chart_LabeledPoints);

        }

        public void SaveToCsv()
        {
            SetSavePath();
            SaveMidlineCsvFile_override();
            SaveItuPointsCsvFile_override();

            //SaveMidlineCsvFile();
            //SaveItuPointsCsvFile();
            //var a = GetPath();
        }

        private void SetSavePath()
        {
            //set global test parameters for mask, wafer, and chip ID. In practive, these must match the device being tested
            //but I will use dummy names for example
            //These must be set so that csv file names are correct
            modGlobalTestParams.MaskName = "DO123";
            modGlobalTestParams.WaferName = "TM346";
            modGlobalTestParams.ChipName = "T7891";
            modGlobalTestParams.OeskID = "SW_EXAMPLE";
            modGlobalTestParams.Tec1ActualTemp = 55;
            modFolderAndFileAndImageNaming.CurrentDateTime = DateTime.Now;
            modGlobals.PATH_TO_TEST_ANALYSIS = Application.StartupPath + "\\Data\\Coarse_tuning";

        }

        private void SetSavePath(string SerialNumber, string MaskName, string WaferName, string ChipName, string OeskID, double temp, DateTime time)
        {
            //set global test parameters for mask, wafer, and chip ID. In practive, these must match the device being tested
            //but I will use dummy names for example
            //These must be set so that csv file names are correct
            modGlobalTestParams.MaskName = MaskName;
            modGlobalTestParams.WaferName = WaferName;
            modGlobalTestParams.ChipName = ChipName;
            modGlobalTestParams.OeskID = OeskID;
            modGlobalTestParams.Tec1ActualTemp = temp;
            modFolderAndFileAndImageNaming.CurrentDateTime = time;
            modGlobals.PATH_TO_TEST_ANALYSIS = Application.StartupPath + $"\\Data\\{SerialNumber}\\Coarse_tuning";
            //modGlobals.PATH_TO_TEST_ANALYSIS = $"D:\\Coarse_tuning\\";
        }
        public void SaveToCsv(string SerialNumber, string MaskName, string WaferName, string ChipName, string OeskID, double temp, DateTime time)
        {
            SetSavePath(SerialNumber, MaskName, WaferName, ChipName, OeskID, temp, time);
            SaveMidlineCsvFile_override();
            //SaveItuPointsCsvFile();
            SaveItuPointsCsvFile_override();
        }

        public string GetPath()
        {
            //string path = $"{modGlobals.PATH_TO_TEST_ANALYSIS}{modGlobalTestParams.MaskName}\\{modGlobalTestParams.WaferName}" +
            //    $"\\{modGlobalTestParams.ChipName}\\({modGlobalTestParams.OeskID})\\CoarseTuning\\" +
            //    $"{modGlobalTestParams.MaskName}_{modGlobalTestParams.WaferName}_{modGlobalTestParams.ChipName}" +
            //    $"_CoarseTuning#Deviations@{modGlobalTestParams.Tec1ActualTemp.ToString("0.00")}C_" +
            //    $"{modFolderAndFileAndImageNaming.CurrentDateTime.ToString("yyyy-MM-dd_HH-mm-ss")}.csv";
            //return path;

            return DeviationsPath;
        }

        public void SaveMidlineCsvFile_override()
        {
            try
            {
                clsMidlineCsvHandler midlineInfoWriter = new clsMidlineCsvHandler();
                var csvRows = BuildMidlineCsvRows();

                if (csvRows != null && csvRows.Count > 0)
                {
                    //midlineInfoWriter.WriteMidlineDataToCsvV2(modFolderAndFileAndImageNaming.COARSE_TUNING_FOLDERNAME, modFolderAndFileAndImageNaming.COARSE_TUNING_FILENAME, false, "", csvRows, modFolderAndFileAndImageNaming.COARSE_TUNING_MIDLINE_INFO_NAME);

                    string path = $"{modGlobals.PATH_TO_TEST_ANALYSIS}";
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    string filePath = $"{modGlobals.PATH_TO_TEST_ANALYSIS}\\{modFolderAndFileAndImageNaming.COARSE_TUNING_MIDLINE_INFO_NAME}.csv";
                    using (StreamWriter Writer = new StreamWriter(filePath))
                    {
                        try
                        {
                            foreach (List<string> currentRow in csvRows)
                            {
                                try
                                {
                                    foreach (string item in currentRow)
                                    {
                                        Writer.WriteLine(item);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    throw;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            throw;
                        }
                    }


                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void SaveItuPointsCsvFile_override()
        {
            try
            {
                clsItuChannelCsvHandler ituChannelWriter = new clsItuChannelCsvHandler();
                ituChannelWriter.CoarseTuningMode = true; //set this to true so the proper column header is used in the csv file.
                List<List<string>> rowsToWrite = BuildItuChannelCsvRows();
                if (rowsToWrite != null && rowsToWrite.Count > 0)
                {
                    //ituChannelWriter.WriteItuChannelDataToCsvV2(modFolderAndFileAndImageNaming.COARSE_TUNING_FOLDERNAME,
                    //    modFolderAndFileAndImageNaming.COARSE_TUNING_FILENAME, false, "",
                    //    rowsToWrite, modFolderAndFileAndImageNaming.COARSE_TUNING_CHANNEL_INFO_NAME);

                    string headerRow = string.Join(",", COARSE_TUNE_RESULT_COLUMN_NAMES);
                    string path = $"{modGlobals.PATH_TO_TEST_ANALYSIS}";
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    string filePath = $"{modGlobals.PATH_TO_TEST_ANALYSIS}\\{modFolderAndFileAndImageNaming.COARSE_TUNING_CHANNEL_INFO_NAME}.csv";
                    DeviationsPath = filePath;
                    using (StreamWriter Writer = new StreamWriter(filePath, false, Encoding.GetEncoding("gb2312")))
                    {
                        Writer.WriteLine(headerRow);
                        try
                        {
                            foreach (List<string> currentRow in rowsToWrite)
                            {
                                Writer.WriteLine(string.Join(",", currentRow));
                            }
                        }
                        catch (Exception ex)
                        {
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        private string DeviationsPath { get; set; }

        private static List<string> COARSE_TUNE_RESULT_COLUMN_NAMES = new List<string>
        {
            "CH",
            "Itu_WL",
            "Itu_Freq",
            "Gain[mA]",
            "SOA1[mA]",
            "SOA2[mA]",
            "Mirror1[mA]",
            "Mirror2[mA]",
            "L_Phase[mA]",
            "Phase1[mA]",
            "Phase2[mA]",
            "Wavelength[nm]",
            "Deviation[nm]",
            "SMSR[dB]",
            "Power[dBm]",
            "MZM1[V]",
            "MZM2[V]",
            "MidlineIndex"
        };


    }
}
