Nutrition tracker app

The app is built in .Net Maui which is a cross platform app.
My app is nutrition tracker app that helps the user track their calories for what they have had for the day. This app allows the user to manually add there 
calories or scan barcodes of food or drinks to recieve their calories from a API. The data is stored using firebase and user authentication data is saved on firebase as well.
It has multiple user accessibility features that follow the WCAG guidelines such as dark mode, adjustable font size toggle on or off features such as haptic feedback.

Features 
- User authentication such as email/password for sign up and login in. this is saved on firebase.
- Secure storage of Firebase UID using Securestorage.
- Logout option is there for the user in the toolbar.
- user can manually log food items such as foodname,calories,carbs,fats. this is saved in firestore database.
- Scan barcodes using the camera and it looks through openfoodfacts API and retrives nutritional information for the barcode.
- view total daily calories and a list of the logged food 
- Profile page displays the saved nutrition entires from the user and grouped together using the date.
- swipe left on a profile to delete individual entries and this also gets deleted from the firebase database. swipe right on a profile to edit a profiles information and then this gets updated in the firebase database.
- Accessbility features such as screen reader using semanticproperties. Also WCAG aligned changing on the font. UI adapts to tablet and phone. other feautres such as toggling on and off for the hardware features such as haptic feddback, text to speech.
- user can also use the camera to add a profile picture.

Technologies used
- .Net MAUI (8.0)
- Firebase authentication
- Firebase Database
- Openfoodfacts API 
- Communitytoolkit.mvvm
- Zxing.Net.MAUI used for barcode scanning 
- secure storage 

Mobile Hardware used
- Camera (used for barcode scannnig and for taking a profile picture)
- Text to speech (used for the total calories for the day when saved to profile)
- Vibration (used when scanning a sucessfull barcode and also used in login if user signs up incorrectly)
- Haptic feedback (used on buttons duing login/sign up)

Future Work
- In the future I would like to add a analytics page where the user could you see trends in their calories over periods of time.
- maybe add some goals for the user
- add support for different languages
- deploy the app to the google playstore
  
Deployment Instructions (only for andorid emulator)
1 - First make sure you have visual studio installed 2022 or later and this needs to be installed with .NET MAUI selected. Download Android SDK and emulator. you can use android device manager or android studio 
2 - I used android device manager so I will instruct you on that. Open andorid device manager and click on new and create your own pixel virtual device. start this emulator
3 - now in visual studio select your emulator in the device dropdown and now press run. The app will run in the android emulator.

Deploying to a physical Android device
1 - on your andorid phone go to settings and scroll down to about phone. Tab build number in about 7 times to go into developer mode.
2 - now go out of about phone and below that should be developer options. Click that and scroll down to enable usb debugging.
3 - Plug your phone using a usb into your laptop and click yes if your computer asks do you recognise this phone. in your toolbar click on the dropdown and go to android local devices and click your phone.
4 - Now run it and the app should deploy to your physical phone

Developed By Abuzarr Ahmed
