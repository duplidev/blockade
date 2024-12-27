namespace Structure.Impl {
    public class VBLStructure : Structure {
        public void Awake() {
            Grid = new byte[,] {
                { 5, 0, 0, 0, 0, 0, }, 
                { 6, 0, 0, 0, 0, 0 }, 
                { 10, 3, 3, 3, 3, 4 }
            };
        }
    }
}