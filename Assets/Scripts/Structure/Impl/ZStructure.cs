namespace Structure.Impl {
    public class ZStructure : Structure {
        public void Awake() {
            Grid = new byte[,] {
                { 0, 0, 8, 3, 4 },
                { 2, 3, 11, 0, 0 }
            };
        }
    }
}