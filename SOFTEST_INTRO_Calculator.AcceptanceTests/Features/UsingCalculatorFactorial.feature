@Factorial
Feature: UsingCalculatorFactorial
    In order to avoid mistakes
    As a calculator user
    I want to be told the factorial of a number

    Scenario: Factorial of a number
        Given I have a calculator
        When I have entered 5 into the calculator and press factorial
        Then the factorial result should be 120

    Scenario Outline: Factorial of special cases
        Given I have a calculator
        When I have entered <value> into the calculator and press factorial
        Then the factorial result should be <result>

        Examples:
            | value | result |
            | 0     | 1      |
            | 5     | 120    |
            
    Scenario Outline: Factorial of unsupported numbers
        Given I have a calculator
        When I have entered <value> into the calculator and press factorial
        Then factorial should be rejected

        Examples:
            | value |
            | 21    |
