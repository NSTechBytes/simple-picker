; SimplePicker Installer Script
; Created with NSIS (Nullsoft Scriptable Install System)

!define APPNAME "SimplePicker"
!define COMPANYNAME "nstechbytes"
!define DESCRIPTION "SimplePicker Application"
!define VERSIONMAJOR 1
!define VERSIONMINOR 0
!define VERSIONBUILD 0
!define HELPURL "http://www.yourcompany.com/support"
!define UPDATEURL "http://www.yourcompany.com/updates"
!define ABOUTURL "http://www.yourcompany.com/about"


; Include Modern UI
!include "MUI2.nsh"
!include "LogicLib.nsh"

; General settings
Name "${APPNAME}"
OutFile "Setup\SimplePicker.exe"
InstallDir "$PROGRAMFILES\${APPNAME}"
InstallDirRegKey HKCU "Software\${APPNAME}" ""
RequestExecutionLevel admin

; Interface Configuration
!define MUI_HEADERIMAGE
!define MUI_HEADERIMAGE_RIGHT
!define MUI_HEADERIMAGE_BITMAP "images\header.bmp"  ; BMP image for header (right side)
!define MUI_WELCOMEFINISHPAGE_BITMAP "images\wizard.bmp"  ; Large image on welcome/finish pages
!define MUI_UNWELCOMEFINISHPAGE_BITMAP "images\wizard.bmp"  ; Large image on uninstaller welcome/finish pages
!define MUI_ICON "images\icon.ico"
!define MUI_UNICON "images\icon.ico"
!define MUI_ABORTWARNING

; Pages
!insertmacro MUI_PAGE_WELCOME
!insertmacro MUI_PAGE_LICENSE "License.txt"
!insertmacro MUI_PAGE_COMPONENTS
!insertmacro MUI_PAGE_DIRECTORY
!insertmacro MUI_PAGE_INSTFILES
!insertmacro MUI_PAGE_FINISH

!insertmacro MUI_UNPAGE_WELCOME
!insertmacro MUI_UNPAGE_CONFIRM
!insertmacro MUI_UNPAGE_INSTFILES
!insertmacro MUI_UNPAGE_FINISH

; Languages
!insertmacro MUI_LANGUAGE "English"

; Version Information
VIProductVersion "${VERSIONMAJOR}.${VERSIONMINOR}.${VERSIONBUILD}.0"
VIAddVersionKey "ProductName" "${APPNAME}"
VIAddVersionKey "CompanyName" "${COMPANYNAME}"
VIAddVersionKey "LegalCopyright" "© ${COMPANYNAME}"
VIAddVersionKey "FileDescription" "${DESCRIPTION}"
VIAddVersionKey "FileVersion" "${VERSIONMAJOR}.${VERSIONMINOR}.${VERSIONBUILD}.0"

; Default installation section
Section "Core Application" SecCore
    SectionIn RO  ; Read-only section (always installed)
    
    SetOutPath $INSTDIR
    
    ; Install application files
    File "..\bin\x86\Release\net8.0-windows\simple-picker.exe"
    File "..\bin\x86\Release\net8.0-windows\simple-picker.dll"
    File "..\bin\x86\Release\net8.0-windows\simple-picker.deps.json"
    File "..\bin\x86\Release\net8.0-windows\simple-picker.runtimeconfig.json"
    File "..\bin\x86\Release\net8.0-windows\settings.json"
    
    ; Install resources directory
    SetOutPath "$INSTDIR\resources"
    File /r "..\bin\x86\Release\net8.0-windows\resources\*.*"
    
    ; Reset output path to main directory
    SetOutPath $INSTDIR
    
    ; Registry entries
    ReadRegStr $0 HKLM "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\${APPNAME}" "UninstallString"
    ${If} $0 != ""
        MessageBox MB_YESNO|MB_ICONQUESTION "A previous version of ${APPNAME} is installed. Would you like to uninstall it first?" IDNO +3
        ExecWait '$0 _?=$INSTDIR'
        Delete $0
    ${EndIf}
    
    ; Create SimplePicker application registry entries
    WriteRegStr HKCU "Software\${APPNAME}" "AppName" "${APPNAME}"
    WriteRegStr HKCU "Software\${APPNAME}" "Version" "${VERSIONMAJOR}.${VERSIONMINOR}"
    WriteRegStr HKCU "Software\${APPNAME}" "Publisher" "${COMPANYNAME}"
    
    ; Create uninstaller
    WriteUninstaller "$INSTDIR\uninstall.exe"
    
    ; Registry information for add/remove programs
    WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APPNAME}" "DisplayName" "${APPNAME}"
    WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APPNAME}" "UninstallString" "$\"$INSTDIR\uninstall.exe$\""
    WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APPNAME}" "QuietUninstallString" "$\"$INSTDIR\uninstall.exe$\" /S"
    WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APPNAME}" "InstallLocation" "$\"$INSTDIR$\""
    WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APPNAME}" "DisplayIcon" "$\"$INSTDIR\simple-picker.exe$\""
    WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APPNAME}" "Publisher" "${COMPANYNAME}"
    WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APPNAME}" "HelpLink" "${HELPURL}"
    WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APPNAME}" "URLUpdateInfo" "${UPDATEURL}"
    WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APPNAME}" "URLInfoAbout" "${ABOUTURL}"
    WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APPNAME}" "DisplayVersion" "${VERSIONMAJOR}.${VERSIONMINOR}.${VERSIONBUILD}"
    WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APPNAME}" "VersionMajor" ${VERSIONMAJOR}
    WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APPNAME}" "VersionMinor" ${VERSIONMINOR}
    WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APPNAME}" "NoModify" 1
    WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APPNAME}" "NoRepair" 1
    
    ; Store installation folder in registry
    WriteRegStr HKCU "Software\${APPNAME}" "" $INSTDIR
SectionEnd

; Optional Desktop Shortcut
Section "Desktop Shortcut" SecDesktop
    CreateShortcut "$DESKTOP\${APPNAME}.lnk" "$INSTDIR\simple-picker.exe" "" "$INSTDIR\simple-picker.exe" 0
SectionEnd

; Optional Start Menu Shortcuts
Section "Start Menu Shortcuts" SecStartMenu
    CreateDirectory "$SMPROGRAMS\${APPNAME}"
    CreateShortcut "$SMPROGRAMS\${APPNAME}\${APPNAME}.lnk" "$INSTDIR\simple-picker.exe" "" "$INSTDIR\simple-picker.exe" 0
    CreateShortcut "$SMPROGRAMS\${APPNAME}\Uninstall.lnk" "$INSTDIR\uninstall.exe" "" "$INSTDIR\uninstall.exe" 0
SectionEnd

; Section descriptions
!insertmacro MUI_FUNCTION_DESCRIPTION_BEGIN
    !insertmacro MUI_DESCRIPTION_TEXT ${SecCore} "Core application files (required)"
    !insertmacro MUI_DESCRIPTION_TEXT ${SecDesktop} "Create a shortcut on the desktop"
    !insertmacro MUI_DESCRIPTION_TEXT ${SecStartMenu} "Create shortcuts in the Start Menu"
!insertmacro MUI_FUNCTION_DESCRIPTION_END

; Uninstaller section
Section "Uninstall"
    ; Remove registry keys
    DeleteRegKey HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APPNAME}"
    DeleteRegKey HKCU "Software\${APPNAME}"
    
    ; Remove files and uninstaller
    Delete "$INSTDIR\simple-picker.exe"
    Delete "$INSTDIR\simple-picker.dll"
    Delete "$INSTDIR\simple-picker.deps.json"
    Delete "$INSTDIR\simple-picker.runtimeconfig.json"
    Delete "$INSTDIR\settings.json"
    Delete "$INSTDIR\uninstall.exe"
    
    ; Remove resources directory
    RMDir /r "$INSTDIR\resources"
    
    ; Remove shortcuts
    Delete "$DESKTOP\${APPNAME}.lnk"
    Delete "$SMPROGRAMS\${APPNAME}\${APPNAME}.lnk"
    Delete "$SMPROGRAMS\${APPNAME}\Uninstall.lnk"
    RMDir "$SMPROGRAMS\${APPNAME}"
    
    ; Remove directories if empty
    RMDir "$INSTDIR"
SectionEnd

; Functions
Function .onInit
    ; Check if application is already running
    System::Call 'kernel32::CreateMutexA(i 0, i 0, t "SimplePicker") i .r1 ?e'
    Pop $R0
    ${If} $R0 != 0
        MessageBox MB_OK|MB_ICONEXCLAMATION "The installer is already running."
        Abort
    ${EndIf}
FunctionEnd

Function un.onInit
    MessageBox MB_ICONQUESTION|MB_YESNO|MB_DEFBUTTON2 "Are you sure you want to completely remove ${APPNAME} and all of its components?" IDYES +2
    Abort
FunctionEnd