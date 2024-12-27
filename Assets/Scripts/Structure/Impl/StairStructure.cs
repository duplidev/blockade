namespace Structure.Impl {
    public class StairStructure : Structure {
        public void Awake() {
            Grid = new byte[,] {
                { 0, 0, 0, 5 },
                { 0, 0, 8, 11 },
                { 0, 8, 11, 0 },
                { 8, 11, 0, 0 },
                { 7, 0, 0, 0 }
            };
        }
    }
}