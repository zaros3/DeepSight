using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ChartDirector;

namespace DeepSight {
    public class threedscatter2 : DemoModule
    {
        List<PositionList> PL = new List<PositionList>();
        WinChartViewer winchart;
        int min;
        int max;
        //Name of demo module
        public string getName() { return "3D Scatter Chart (2)"; }

        //Number of charts produced in this demo module
        public int getNoOfCharts() { return 1; }

        //Main code for creating chart.
        //Note: the argument chartIndex is unused because this demo only has 1 chart.
        public void createChart(WinChartViewer viewer, int chartIndex, int minvalue, int maxvalue)
        {
            ThreeDScatterChart c = new ThreeDScatterChart(720, 520); //전체 크기수정(하얀부분)
            winchart = viewer;
            min = minvalue;         //좌표값의 최소값, 최대값을 정할수있게 만들어뒀습니다.
            max = maxvalue;
            
            double[] xData1 = { minvalue, minvalue, minvalue, minvalue };
            double[] yData1 = { minvalue, minvalue, minvalue, minvalue };
            double[] zData1 = { minvalue, minvalue, minvalue, minvalue };

            double[] xData2 = { maxvalue, maxvalue, maxvalue, maxvalue };
            double[] yData2 = { maxvalue, maxvalue, maxvalue, maxvalue };
            double[] zData2 = { maxvalue, maxvalue, maxvalue, maxvalue };
            // 필독===========================================
            //
            // 데이터는 세로방향으로 점하나씩 만들어지게 되있습니다. 한마디로 double[] 하나당 점이 4개씩찍히니 값을 일치시켜주는게 좋습니다.
            //
            //================================================
                     

            
            c.addTitle("3D Scatter Chart (2)  ", "Times New Roman Italic", 20); // 제목, 폰트, 크기

            // Set the center of the plot region at (350, 240), and set width x depth x height to
            // 360 x 360 x 270 pixels

            c.setPlotRegion(350, 240, 360, 360, 270); //이것을 이용해서 그래프의 크기를 조절할수있습니다.
            
            c.setViewAngle(15, 30); // 보는각도입니다.
            
            
            ThreeDScatterGroup g = c.addScatterGroup(xData1, yData1, zData1, "",
                Chart.GlassSphere2Shape, 0, Chart.SameAsMainColor);
            c.addScatterGroup(xData2, yData2, zData2, "",
                Chart.GlassSphere2Shape, 0, Chart.SameAsMainColor); // 점 추가시키는부분

            //g 없어도 잘돌아갑니다.

            for (int i = 0; i < PL.Count(); i++)
            {
                c.addScatterGroup(PL[i].Xd, PL[i].Yd, PL[i].Zd, "",
                Chart.GlassSphere2Shape, 10, Chart.SameAsMainColor);
            }//좌표를 추가하면 List에 저장되서 새로그리는것

            //GlassSphere2Shape
            // Add grey (888888) drop lines to the symbols
            g.setDropLine(0x888888); //뭔지모름 없어도잘돌아감

            // Add a color axis (the legend) in which the left center is anchored at (645, 220). Set
            // the length to 200 pixels and the labels on the right side. Use smooth gradient
            // coloring.

            c.setColorAxis(645, 220, Chart.Left, 200, Chart.Right).setColorGradient(); //우측 그래프바 입니다.

            // Set the x, y and z axis titles using 10 points Arial Bold font
            c.xAxis().setTitle("X-Axis Place Holder", "Arial Bold", 10); 
            c.yAxis().setTitle("Y-Axis Place Holder", "Arial Bold", 10);
            c.zAxis().setTitle("Z-Axis Place Holder", "Arial Bold", 10);

            // Output the chart
            viewer.Chart = c;


            //include tool tip for the chart
            viewer.ImageMap = c.getHTMLImageMap("clickable", "",
                "title='(x={x|p}, y={y|p}, z={z|p})'");
        }
        
        public void AddPointChart(WinChartViewer viewer, int x, int y , int z)
        {
            PL.Add(new PositionList(x, y, z));            
        }
    }
}
