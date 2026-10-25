Feature: Blood pressure category
  As a patient checking my blood pressure
  I want my reading to be put into a category
  So that I know whether my blood pressure is healthy

  Scenario Outline: A valid reading is categorised
    Given a systolic pressure of <systolic>
    And a diastolic pressure of <diastolic>
    When the blood pressure category is calculated
    Then the category should be "<category>"

    Examples:
      | systolic | diastolic | category                |
      | 85       | 55        | Low Blood Pressure      |
      | 110      | 70        | Ideal Blood Pressure    |
      | 125      | 70        | Pre-High Blood Pressure |
      | 110      | 85        | Pre-High Blood Pressure |
      | 150      | 70        | High Blood Pressure     |
      | 130      | 95        | High Blood Pressure     |

  Scenario: Systolic must be greater than diastolic
    Given a systolic pressure of 80
    And a diastolic pressure of 90
    When the reading is submitted
    Then the error "Systolic must be greater than Diastolic" should be shown

  Scenario Outline: A reading outside the allowed range is rejected
    Given a systolic pressure of <systolic>
    And a diastolic pressure of <diastolic>
    When the reading is validated
    Then the error "<error>" should be shown

    Examples:
      | systolic | diastolic | error                   |
      | 200      | 80        | Invalid Systolic Value  |
      | 120      | 30        | Invalid Diastolic Value |
