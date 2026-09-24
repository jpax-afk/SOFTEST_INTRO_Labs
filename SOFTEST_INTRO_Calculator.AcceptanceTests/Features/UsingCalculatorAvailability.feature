@Availability 
Feature: UsingCalculatorAvailability 
    In order to calculate MTBF and Availability 
    As someone who struggles with maths 
    I want to be able to use my calculator to do this 
    
    Scenario: Calculating MTBF 
        Given I have a calculator 
        When I have entered 1000 and 5 into the calculator and press MTBF 
        Then the result should be 200 
        
    Scenario: Calculating Availability 
        Given I have a calculator 
        When I have entered 1000 and 5 into the calculator and press Availability 
        Then the result should be 0.9950248756

    Scenario: Calculating Availability from named reliability values 
        Given I have a calculator 
        And the reliability values are 
        | MTBF | MTTR | 
        | 90 | 10 | 
        When I calculate Availability from these values 
        Then the result should be 0.9

    Scenario: Reject MTBF when there are zero failures
        Given I have a calculator
        When I have entered 1000 and 0 into the calculator and press MTBF
        Then the MTBF calculation should be rejected

    Scenario: Reject availability when the denominator is zero
        Given I have a calculator
        When I have entered 0 and 0 into the calculator and press Availability
        Then the availability calculation should be rejected