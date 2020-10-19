/* Create user to be used by web service for accessing database operations. */

/****
 *** NOTICE 2020/10/20
 ***    It appears the user name is lower cased when it is created in Postgres.
 ***    As a work-around, just use a lower case name.
 ***/
CREATE USER cellarmaster_antiq WITH PASSWORD 'ant1Quar1an!' CREATEDB;
