using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphPlotterAPI.Models;
using OxyPlot;

namespace GraphPlotterAPI.Services.Interfaces
{
    public interface IPlotterService
    {
        PlotModel Plot(FunctionsPlotModel Model);
    }
}
