namespace Structure.Impl {
    public class WSLStructure : Structure {
        private void Awake() {
            Grid = new byte[,] {
                { 0, 0, 0, 5, },
                { 2, 3, 3, 11 }
            };
        }
    }
}