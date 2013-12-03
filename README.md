Asset Store Installer
===========

Notes:
---
VERY IMPORTANT: Do not test this installer within its own development environment.
It will DELETE ITSELF after running!
You can define ASINSTALLER_DEVELOPMENT in order to prevent the installer window from showing up on load.

This installer will not work for Unity versions under 3.3, as it uses AssetDatabase.ImportPackage.

Usage:
1. Import these files into the project that will be uploaded to the Asset Store.
2. In all files/file names, rename the following:
    * SHORT_PACKAGE_NAME - A short name for your package to be used in variable and file names
    * FULL_PACKAGE_NAME - A full, human readable name for your package
4. Be sure to edit the included key image and the README.txt.
5. Add any additional pre- and post- callbacks that your package may require.

License
---
Copyright (c) 2013 Thinksquirrel Software, LLC
All rights reserved.

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, 
BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT 
SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL 
DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) 
HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN 
ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

