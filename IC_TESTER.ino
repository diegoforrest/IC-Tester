#include <AccelStepper.h>

// Pin Definitions for Joysticks
#define JOYSTICK1_X_PIN 39  // X-axis pin for joystick 1 (Motor 1 & 2)
#define JOYSTICK1_Y_PIN 36  // Y-axis pin for joystick 1 (Motor 1 & 2)
#define JOYSTICK2_X_PIN 32  // X-axis pin for joystick 2 (Motor 3 & 4)
#define JOYSTICK2_Y_PIN 35  // Y-axis pin for joystick 2 (Motor 3 & 4)

// Stepper Pins for Motors 1 & 2
#define STEP1_PIN 22        // Step pin for motor 1
#define DIR1_PIN 23         // Direction pin for motor 1
#define STEP2_PIN 21        // Step pin for motor 2
#define DIR2_PIN 19         // Direction pin for motor 2

// Stepper Pins for Motors 3 & 4
#define STEP3_PIN 18        // Step pin for motor 3
#define DIR3_PIN 5         // Direction pin for motor 3
#define STEP4_PIN 17        // Step pin for motor 4
#define DIR4_PIN 16        // Direction pin for motor 4

// Stepper Configuration
AccelStepper stepper1(AccelStepper::DRIVER, STEP1_PIN, DIR1_PIN);
AccelStepper stepper2(AccelStepper::DRIVER, STEP2_PIN, DIR2_PIN);
AccelStepper stepper3(AccelStepper::DRIVER, STEP3_PIN, DIR3_PIN);
AccelStepper stepper4(AccelStepper::DRIVER, STEP4_PIN, DIR4_PIN);

//thresholds for joystick X and Y
const int NEUTRAL_LOW = 1700;  // Lower bound for neutral zone
const int NEUTRAL_HIGH = 2300; // Upper bound for neutral zone

// Stepper speed settings
const int MAX_SPEED = 5000; // Maximum speed in steps per second
const int SPEED_INCREMENT = 2000; // Speed increment for faster movement

void setup() {
  Serial.begin(9600);  // Start serial communication

  // Initialize the stepper motors
  stepper1.setMaxSpeed(MAX_SPEED);
  stepper1.setAcceleration(3000);
  stepper2.setMaxSpeed(MAX_SPEED);
  stepper2.setAcceleration(3000);
  stepper3.setMaxSpeed(MAX_SPEED);
  stepper3.setAcceleration(3000);
  stepper4.setMaxSpeed(MAX_SPEED);
  stepper4.setAcceleration(3000);
}

void loop() {
  // Joystick 1 for Motor 1 & 2
  int x1Value = analogRead(JOYSTICK1_X_PIN);
  int y1Value = analogRead(JOYSTICK1_Y_PIN);
  
  if (x1Value < NEUTRAL_LOW) {
    stepper1.setSpeed(SPEED_INCREMENT);  // Motor 1 left
  } else if (x1Value > NEUTRAL_HIGH) {
    stepper1.setSpeed(-SPEED_INCREMENT);   // Motor 1 right
  } else {
    stepper1.setSpeed(0);  // Stop Motor 1
  }

  if (y1Value < NEUTRAL_LOW) {
    stepper2.setSpeed(-SPEED_INCREMENT);  // Motor 2 up
  } else if (y1Value > NEUTRAL_HIGH) {
    stepper2.setSpeed(SPEED_INCREMENT);   // Motor 2 down
  } else {
    stepper2.setSpeed(0);  // Stop Motor 2
  }

  // Joystick 2 for Motor 3 & 4
  int x2Value = analogRead(JOYSTICK2_X_PIN);
  int y2Value = analogRead(JOYSTICK2_Y_PIN);

  if (x2Value < NEUTRAL_LOW) {
    stepper3.setSpeed(SPEED_INCREMENT);  // Motor 3 left
  } else if (x2Value > NEUTRAL_HIGH) {
    stepper3.setSpeed(-SPEED_INCREMENT);   // Motor 3 right
  } else {
    stepper3.setSpeed(0);  // Stop Motor 3
  }

  if (y2Value < NEUTRAL_LOW) {
    stepper4.setSpeed(-SPEED_INCREMENT);  // Motor 4 up
  } else if (y2Value > NEUTRAL_HIGH) {
    stepper4.setSpeed(SPEED_INCREMENT);   // Motor 4 down
  } else {
    stepper4.setSpeed(0);  // Stop Motor 4
  }

  // Run all motors
  stepper1.runSpeed();
  stepper2.runSpeed();
  stepper3.runSpeed();
  stepper4.runSpeed();

  delay(5);
}