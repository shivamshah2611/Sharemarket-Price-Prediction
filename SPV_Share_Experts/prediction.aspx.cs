using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Web.UI.HtmlControls;

public partial class prediction : System.Web.UI.Page
{
    const string apiKey = "V741TEC42OB1WL5N";
    List<double> closeIntraDay;
    List<string> timestampIntraDay;
    List<double> closeDay;
    List<string> timestampDay;
    List<double> predicted;

    protected void Page_Load(object sender, EventArgs e)
    {
        closeDay = new List<double>();
        closeIntraDay = new List<double>(); 
        timestampDay = new List<string>();
        timestampIntraDay = new List<string>();
        predicted = new List<double>();

        if (Session["id"] != null)
        {
            saveBTN.Visible = true;
            deleteBTN.Visible = true;
            CodesList.Visible = true;
            CodesList.Items.Clear();
                List<string> codes = new DatabaseOperations(Server.MapPath("~")).GetCodes(Session["id"].ToString());

            foreach (string name in codes)
                {
                    CodesList.Items.Add(new ListItem(name));
                }
            ListItem myDefaultItem = new ListItem("(Select Share Code)", string.Empty);
            myDefaultItem.Selected = true;
            CodesList.Items.Insert(0, myDefaultItem);
            // Response.Write(Session["id"]);
        }
        else
        {
            saveBTN.Visible = false;
            CodesList.Visible = false;
            deleteBTN.Visible = false;
        }
        
    }
    protected void predict_Click(object sender, EventArgs e)
    {
        try
        {
            GetCSV(stockcodeTXT.Text.Trim());
            
         
            Analysis();

            MakeChart();

            closeDay.Clear();
            closeIntraDay.Clear();
            timestampIntraDay.Clear();
            timestampDay.Clear();
        }
        catch(CoolDownExcpetion ex)
        {
            Session["ex"] = ex.Message;
            Console.WriteLine(ex);
            Response.Redirect("ErrorPage.aspx");
        }
        catch (Exception ex)
        {
            Session["ex"] = ex.Message;
            Console.WriteLine(ex);
            Response.Redirect("ErrorPage.aspx");
        }
    }
    public void GetCSV(string stockcode)
    {
        string line;
        string[] splitString;
        string[] urls = MakeUrl(stockcode);
        int j = 0;
        foreach (string url in urls)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
           
            if (resp.ContentType.Equals("application/x-download"))
            {
                StreamReader sr = new StreamReader(resp.GetResponseStream());

                int i = 0;

                string[] line1=sr.ReadLine().Split(',');
                string closing = line1[4];

                while (!sr.EndOfStream)
                {
                    line = sr.ReadLine();
                    splitString = line.Split(',');
                    if (i > 0 && j == 0)
                    {
                        closeDay.Add(double.Parse(splitString[4]));
                        timestampDay.Add(splitString[0]);
                    }
                    else if (i > 0 && j > 0)
                    {
                        closeIntraDay.Add(double.Parse(splitString[4]));
                        timestampIntraDay.Add(splitString[0].Split(' ')[1]);
                    }

                    i++;
                }
                sr.Close();
                j++;
            }


            else
            {
                throw new CoolDownExcpetion("Please cool down API does not support this much consecutive calls");
            }
        }

    }
    string[] MakeUrl(string stockCode)
    {
        string baseUrl = "https://www.alphavantage.co/";


        string query1 = baseUrl + "query?function=TIME_SERIES_DAILY&symbol=" + stockCode + "&apikey=" + apiKey + "&datatype=csv";

        string query2 = baseUrl + "query?function=TIME_SERIES_INTRADAY&symbol=" + stockCode + "&interval=30min&apikey=" + apiKey + "&datatype=csv";

        string[] s = { query1, query2 };

        return s;

    }
    void MakeChart()
    {
        //100 Day Chart
        var shareChart1 = new Chart();
        shareChart1.Titles.Add("Last 100 Day Prices");
        shareChart1.Width = 1000;

        ChartArea chartArea1 = new ChartArea("PricesDay");
        chartArea1.AxisX.Title = "Time";
        chartArea1.AxisY.Title = "Price in $";

        shareChart1.ChartAreas.Add(chartArea1);

        var seriesClose = new Series("close");
        seriesClose.ChartType = SeriesChartType.Line;
        seriesClose.Color = Color.Green;

        timestampDay.Reverse();
        closeDay.Reverse();

        seriesClose.Points.DataBindXY(timestampDay, closeDay);
        shareChart1.Series.Add(seriesClose);

        //Today's Chart
        seriesClose.ChartArea = "PricesDay";
        var shareChart2 = new Chart();
        shareChart2.Titles.Add("Today's Prices");
        shareChart2.Width = 1000;

        ChartArea chartArea2 = new ChartArea("PricesIntraday");
        chartArea2.AxisX.Interval = 10;
        chartArea1.AxisX.Title = "Time";
        chartArea1.AxisY.Title = "Price in $";

        shareChart2.ChartAreas.Add(chartArea2);

        var seriesIntraClose = new Series("closeIntra");
        seriesIntraClose.ChartType = SeriesChartType.Line;

        timestampIntraDay.Reverse();
        closeIntraDay.Reverse();

        seriesIntraClose.Points.DataBindXY(timestampIntraDay, closeIntraDay);
        shareChart2.Series.Add(seriesIntraClose);


        //Predicted Prices Chart
        var shareChart3 = new Chart();
        shareChart3.Titles.Add("Predicted Prices");
        shareChart3.Width = 1000;

        ChartArea chartArea3 = new ChartArea("PricesPredicted");
        chartArea3.AxisY.Title = "Price in $";
        chartArea3.AxisX.Title = "Date";
        chartArea3.AxisX.Interval = 1;

        shareChart3.ChartAreas.Add(chartArea3);

        var seriesPredicted = new Series("predictedClose");
        seriesPredicted.ChartType = SeriesChartType.Line;

        List<DateTime> predictedTime = new List<DateTime>();
        for(int i=0;i<10;i++)
        {
            predictedTime.Add(DateTime.Now.AddDays(i));
        }
        seriesPredicted.Points.DataBindXY(predictedTime, predicted);
        shareChart3.Series.Add(seriesPredicted);


        //Add Chart To Place Holder
        PlaceHolder1.Controls.Add(shareChart1);
        PlaceHolder1.Controls.Add(shareChart2);
        PlaceHolder1.Controls.Add(shareChart3);
        PlaceHolder1.DataBind();
    }

    void Analysis()
    {
        List<double> changeDay = new List<double>();//diff in last 100 day prices
        List<double> lessthan0Day = new List<double>();//less than 0 in change
        List<double> morethan0Day = new List<double>();

        List<double> changeIDay = new List<double>();//todays change
        List<double> lessthan0IDay = new List<double>();
        List<double> morethan0IDay = new List<double>();


        for (int i = 0; i < closeDay.Count - 1; i++)
        {
            changeDay.Add((closeDay[i] - closeDay[i + 1]));
        }

        for (int i = 0; i < closeIntraDay.Count - 1; i++)
        {
            changeIDay.Add((closeIntraDay[i] - closeIntraDay[i + 1]));
        }

        foreach (double s in changeDay)
        {
            if (s < 0)
                lessthan0Day.Add(s);
            else
                morethan0Day.Add(s);
        }

        foreach (double s in changeIDay)
        {
            if (s < 0)
                lessthan0IDay.Add(s);
            else
                morethan0IDay.Add(s);
        }

        double countupDay = morethan0Day.Count;
        double countdownDay = lessthan0Day.Count;

        double countupIDay = morethan0IDay.Count;
        double countdownIDay = lessthan0IDay.Count;

        double avgupDay = morethan0Day.Average() * countupDay;
        double avgdownDay = lessthan0Day.Average() * countdownDay;
       
        double avgupIDay = morethan0IDay.Average() * countupIDay;
        double avgdownIDay = lessthan0IDay.Average() * countdownIDay;

        double avgchangeDay = (avgupDay + avgdownDay) / (countupDay + countdownDay);
        double avgchangeIDay = (avgupIDay + avgdownIDay) / (countupIDay + countdownIDay);




        Label lbl1 = new Label();
        Label lbl2 = new Label();
        Label lbl3 = new Label();
        
        lbl1.Text = "Average Change Per Day: " + avgchangeDay.ToString("0.#####");
        lbl2.Text = "Average Change Today: " + avgchangeIDay.ToString("0.#####");
        
        PlaceHolder2.Controls.Add(lbl1);
        PlaceHolder2.Controls.Add(new HtmlGenericControl("br"));
        PlaceHolder2.Controls.Add(lbl2);
        PlaceHolder2.Controls.Add(new HtmlGenericControl("br"));

        double lastPrice = closeDay[0];

        lbl3.Text = "Today(" + timestampDay[0] + ")'s Price: " + lastPrice.ToString("0.####") ;
        PlaceHolder2.Controls.Add(lbl3);
        PlaceHolder2.Controls.Add(new HtmlGenericControl("br")); 

        bool chanceUp;

        double recentUp = 0;
        double recentDown = 0;

        for(int i=0;i<10;i++)
        {
            if(closeDay[i]>0)
            {
                recentUp += 1;
            }
            else
            {
                recentDown += 1;
            }
        }

        double tempUp = recentUp;
        double tempDown = recentDown;

        if (avgchangeIDay > 0)
        {
            chanceUp = true;
        }
        else
        {
            chanceUp = false;
        }



        for (int i = 0; i < 10; i++)
        {
            if (chanceUp)
            {
                lastPrice += morethan0Day.Average();
                predicted.Add(lastPrice);
                if(tempUp>5)
                {
                    tempUp -= 5;
                    chanceUp = true;
                }
                else
                {
                    tempUp = recentUp;
                    chanceUp = false;
                }
            }
            else
            {
                lastPrice += lessthan0Day.Average();
                predicted.Add(lastPrice);
                if (tempDown > 5)
                {
                    tempDown -= 5;
                    chanceUp = false;
                }
                else 
                {
                    tempDown = recentDown;
                    chanceUp = true;
                }
            }
        }

        TextBox predictedTXT = new TextBox();
        predictedTXT.TextMode = TextBoxMode.MultiLine;
        predictedTXT.ReadOnly = true;
        predictedTXT.Height = 390;
        predictedTXT.Width = 350;

        predictedTXT.Text = "Predicted Price over next 10 Days: " + Environment.NewLine;
        foreach(double p in predicted)
        {
            predictedTXT.Text += p.ToString("0.####") + Environment.NewLine;
        }

        PlaceHolder3.Controls.Add(predictedTXT);

    }

   

    protected void saveBTN_Click(object sender, EventArgs e)
    {
        try
        {
            
            DatabaseOperations db = new DatabaseOperations(Server.MapPath("~"));
            int i=db.InsertCode(stockcodeTXT.Text.Trim(),Session["id"].ToString());
            
            if(i==1)
            {
                Response.Redirect("prediction.aspx", false);
                Context.ApplicationInstance.CompleteRequest();

            }
            else
            {
                Response.Write("<script>alert('Unable to Add Code!')</script>");
            }
        }
        catch(Exception ex)
        {
            Session["ex"] = ex.Message;
            Response.Redirect("ErrorPage.aspx");
        }
    }

    protected void deleteBTN_Click(object sender, EventArgs e)
    {
        try
        {
            
            DatabaseOperations db = new DatabaseOperations(Server.MapPath("~"));
            int i = db.DeleteCode(stockcodeTXT.Text.Trim(), Session["id"].ToString());

            if (i == 1)
            {
                Response.Write("<script>alert('Successfully Deleted Code!')</script>");
                Response.Redirect("prediction.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
            }
            else
            {
                Response.Write("<script>alert('Unable to Delete Code!')</script>");
            }
        }
        catch (Exception ex)
        {
            Session["ex"] = ex.Message;
            Response.Redirect("ErrorPage.aspx");
        }
    }
}