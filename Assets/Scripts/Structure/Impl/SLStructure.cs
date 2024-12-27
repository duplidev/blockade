namespace Structure.Impl {
    public class SlStructure : Structure {
        public void Awake() {
            Grid = new byte[,] {
                { 0, 5, },
                { 2, 11 }
            };
        }
    }
}