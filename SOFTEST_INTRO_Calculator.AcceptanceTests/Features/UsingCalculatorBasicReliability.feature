@BasicMusa
Feature: UsingCalculatorBasicReliability
    In order to calculate the Basic Musa model's failures and intensities
    As a Software Quality Metric enthusiast
    I want to use my calculator to do this

    The examples use hours as the execution-time unit and failures per hour
    as the failure-intensity unit. The model assumes a finite expected failure
    total and a constant reduction in failure intensity after corrected failures.

    Scenario: Calculate current failure intensity after five execution hours
        Given I have a calculator
        And the initial failure intensity is 10 failures per hour
        And the expected total number of failures is 100
        And the accumulated execution time is 5 hours
        When I calculate the current failure intensity
        Then the result should be 6.0653065971

    Scenario: Calculate expected cumulative failures after five execution hours
        Given I have a calculator
        And the initial failure intensity is 10 failures per hour
        And the expected total number of failures is 100
        And the accumulated execution time is 5 hours
        When I calculate the expected cumulative failures
        Then the result should be 39.3469340287

    Scenario Outline: Reject invalid Basic Musa inputs
        Given I have a calculator
        And the initial failure intensity is <lambda0> failures per hour
        And the expected total number of failures is <nu0>
        And the accumulated execution time is <time> hours
        When I calculate the current failure intensity
        Then the Basic Musa calculation should be rejected

        Examples:
            | lambda0 | nu0 | time |
            | 0       | 100 | 5    |
            | 10      | 0   | 5    |
            | 10      | 100 | -1   |
