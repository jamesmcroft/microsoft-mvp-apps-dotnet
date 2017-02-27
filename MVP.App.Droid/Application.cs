namespace MVP.App
{
    public static class Application
    {
        private static Locator locator;

        public static Locator Locator => locator ?? (locator = new Locator());
    }
}