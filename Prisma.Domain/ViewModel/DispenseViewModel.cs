using MDFusionLabHaute.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDFusionLabHaute.Domain.ViewModel
{
    public class DispenseViewModel : AuditViewModel
    {
        public int ID { get; set; }
        public decimal? AmountToDispense { get; set; }
        public int? AmountToDispenseUnitId { get; set; }
        public int? ComponentUnitId { get; set; }
        public string ComponentNames { get; set; }
        public string ComponentAmounts { get; set; }
        public string Name { get; set; }
        public string DispenseAmount { get; set; }

        public List<ProductCodeList> productCodeList { get; set; }
        public List<CollectionList> collectionList { get; set; }
        public List<ProductNameList> productNameList { get; set; }
        public List<ProductFormulaList> productFormulaList { get; set; }
        public List<Unit> unitList { get; set; }
        public List<CanSize> canSize { get; set; }

        public DispenseViewModel()
        {
            productCodeList = new List<ProductCodeList>();
            collectionList = new List<CollectionList>();
            productNameList = new List<ProductNameList>();
            productFormulaList = new List<ProductFormulaList>();
            unitList = new List<Unit>();
            canSize = new List<CanSize>();
        }
    }
    public class ProductCodeList
    {
        public int ID { get; set; }
        public string ProductCode { get; set; }
    }

    public class CollectionList
    {
        public int ID { get; set; }
        public string Collection { get; set; }
    }
    public class ProductNameList
    {
        public int ID { get; set; }
        public string ProductName { get; set; }
    }

    public class ProductFormulaList
    {
        public int ID { get; set; }
        public string ProductCode { get; set; }
        public string Collection { get; set; }
        public string ProductName { get; set; }
        public string DispenseAmount { get; set; }
        //public string ColorFormula { get; set; }
        public string ColorCode { get; set; }
        public string Amount { get; set; }
    }
}