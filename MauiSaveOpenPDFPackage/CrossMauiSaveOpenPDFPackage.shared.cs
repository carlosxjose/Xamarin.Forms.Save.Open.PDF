namespace Plugin.MauiSaveOpenPDFPackage
{
    /// <summary>
    /// Cross MauiSaveOpenPDFPackage
    /// </summary>
    public static class CrossMauiSaveOpenPDFPackage
    {
        static Lazy<IMauiSaveOpenPDFPackage> implementation = new Lazy<IMauiSaveOpenPDFPackage>(() => CreateMauiSaveOpenPDFPackage(), System.Threading.LazyThreadSafetyMode.PublicationOnly);

        /// <summary>
        /// Gets if the plugin is supported on the current platform.
        /// </summary>
        public static bool IsSupported => implementation.Value == null ? false : true;

        /// <summary>
        /// Current plugin implementation to use
        /// </summary>
        public static IMauiSaveOpenPDFPackage Current
        {
            get
            {
                IMauiSaveOpenPDFPackage ret = implementation.Value;
                if (ret == null)
                {
                    throw NotImplementedInReferenceAssembly();
                }
                return ret;
            }
        }

        static IMauiSaveOpenPDFPackage CreateMauiSaveOpenPDFPackage()
        {
#if NETSTANDARD1_0 || NETSTANDARD2_0
            return null;
#else

#if ANDROID || IOS || WINDOWS
#pragma warning disable IDE0022 // Use expression body for methods
            return new MauiSaveOpenPDFPackageImplementation();
#pragma warning restore IDE0022 // Use expression body for methods
#else
            return null;
#endif
#endif
        }

        internal static Exception NotImplementedInReferenceAssembly() =>
            new NotImplementedException("This functionality is not implemented in the portable version of this assembly.  You should reference the NuGet package from your main application project in order to reference the platform-specific implementation.");

    }
}
