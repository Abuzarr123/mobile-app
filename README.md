[![Review Assignment Due Date](https://classroom.github.com/assets/deadline-readme-button-22041afd0340ce965d47ae6ef1cefeee28c7c493a6346c4f15d667ab976d596c.svg)](https://classroom.github.com/a/rNdN2Yn1)
# Mobile Computing Assessment 24/25
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
- swipe on a profile to delete individual entries and this also gets deleted from the firebase database
- Accessbility features such as screen reader using semanticproperties. Also WCAG aligned changing on the font. UI adapts to tablet and phone. other feautres such as toggling on and off for the hardware features such as haptic feddback, text to speech.

Technologies used
- .Net MAUI (8.0)
- Firebase authentication
- Firebase Database
- Openfoodfacts API 
- Communitytoolkit.mvvm
- Zxing.Net.MAUI used for barcode scanning 
- secure storage 

Mobile Hardware used
- Camera (used for barcode scannnig)
- Text to speech
- Vibration
- Haptic feedback

