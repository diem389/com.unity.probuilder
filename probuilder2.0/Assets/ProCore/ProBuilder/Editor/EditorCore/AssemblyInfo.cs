﻿#if PROBUILDER_DLL
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Unity.ProBuilder.AddOns.Editor")]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Unity.ProBuilder.Tests")]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Unity.ProBuilder.Editor.Tests")]
#else
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Unity.ProBuilder.AddOns.Editor")]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Unity.ProBuilder.Tests")]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Unity.ProBuilder.Editor.Tests")]
#endif