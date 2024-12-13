# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

## [2.0.0]
- First release version 2.0.0 along side first releases to production for webapi, dashboard, courier. Consumer had first release in 1.0.0.
### Added
- Ability to extract data from mongo and insert into target sql database.
- Supports MySql and SqlServer as target databases. With a simple way to add more providers if needed in the future.
- Ability to set batch size and batch delay seconds.