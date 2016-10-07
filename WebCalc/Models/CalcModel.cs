using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using Domain.Models;

namespace WebCalc.Models
{
    /// <summary>
    /// Модель для расчета значений
    /// </summary>
    public class CalcModel
    {
        public CalcModel()
        {
            //
        }

        public CalcModel(IEnumerable<string> operations)
        {
            Operations = operations.Select(o => new SelectListItem {Text = o, Value = o}).ToList();
        }

        [Required]
        [Display(Name="Первая переменная")]
        public int X { get; set; }

        [Required]
        public int Y { get; set; }

        [Display(Name = "Результат")]
        public double? Result { get; set; }

        public string Operation { get; set; }

        public IList<SelectListItem> Operations { get; set; }

        [UIHint("History")]
        public IEnumerable<History> History { get; set; }
    }
}