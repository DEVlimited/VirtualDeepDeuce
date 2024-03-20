# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

## [1.0.0] - 2022-04-12
### Added
- Finished complete functions of the data toolkit.

## [1.1.0] - 2022-07-1
### Added
- Added SQLUtil, It provides a new function that allows users to dynamically create SQLite compliant database files at runtime.
- Now CSVTable will automatically distinguish between pure CSV and non-pure CSV, and then process the ColumnHeaders field correctly for them.
### Changed
- Change relevant features of CSVTable, Now the first row of csvtable no longer has to be CSV with header type description row.
  Pure CSV string without any header line is OK to use.

## [1.1.1] - 2022-07-21
### Added
- Added Folder utils for editor mode, including file find and copy.

## [1.1.2] - 2022-09-16
### Added
- Added support for IL2CPP.