# Our.Umbraco.MediaProtection

This prevents bad actors from manipulating processing parameters and DOS your site by requesting a large amount of slightly different image URLs that all need to process the image (a potentially CPU and memory expensive process). These processed images are then also saved to the ImageSharp cache (stored in umbraco\Data\TEMP\MediaCache by default), which doesn't have a built-in way to clean up old/infrequently used files and could therefore fill up your disk.

This works in Umbraco 10.x.x.