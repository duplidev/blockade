namespace Structure.Impl {
    public class L3Structure : Structure {
        public void Awake() {
            Grid = new byte[,] {
                { 0, 5, 0, 0 },
                { 8, 15, 3, 4 },
                { 7, 0, 0, 0 }
            };
        }
    }
}