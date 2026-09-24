@Divisions 
Feature: UsingCalculatorDivision 
    In order to conquer divisions 
    As a division enthusiast 
    I want to understand a variety of division operations 

    Scenario Outline: Divide two numbers 
        Given I have a calculator 
        When I have entered <numerator> and <divisor> into the calculator and press divide 
        Then the result should be <quotient> 

        Examples: 
            | numerator | divisor | quotient | 
            | 1 | 2 | 0.5 | 
            | 0 | 15 | 0 | 
            | 15 | -3 | -5 | 

    Scenario Outline: Reject division by zero 
        Given I have a calculator 
        When I have entered <numerator> and 0 into the calculator and press divide 
        Then division should be rejected 

        Examples: 
            | numerator | 
            | 15 | 
            | 0 | 
