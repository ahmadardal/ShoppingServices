namespace InventoryService.Model
{


    public class Product
    {

        public Guid id { get; set; }
        public string name { get; set; }
        public double price { get; set; }
        public string image { get; set; }
        public string category { get; set; }
        public bool available { get; set; }

    }

}