Asset Store Installer
===========

Notes
---
**VERY IMPORTANT**: Do not test this installer within its own development environment.
It will DELETE ITSELF after running!
You can define *ASINSTALLER_DEVELOPMENT* in order to prevent the installer window from showing up on load.

This installer will not work for Unity versions under 3.3, as it uses AssetDatabase.ImportPackage.

Usage:
   1. Import these files into the project that will be uploaded to the Asset Store.
   2. In all files/file names, rename the following:
      * *SHORT_PACKAGE_NAME* - A short name for your package to be used in variable and file names
      * *FULL_PACKAGE_NAME* - A full, human readable name for your package
   3. Be sure to edit the included key image and the README.txt.
   4. Add any additional pre- and post- callbacks that your package may require.

License
---

MIT License, slightly changed to be more compatible with the Unity Asset Store. You don't need to include this with your package.

----

Copyright (c) 2013 Thinksquirrel Software, LLC

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all unmodified copies of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
