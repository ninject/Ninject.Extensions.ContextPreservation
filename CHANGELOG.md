# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/en/1.0.0/)
and this project adheres to [Semantic Versioning](http://semver.org/spec/v2.0.0.html).

## [3.3.1] - 2017-10-22

### Added
- Marked CLS Compliant

## [3.3.0] - 2017-10-05

### Added
 - Support .NET Standard 2.0

### Removed
 - .NET 3.5, .NET 4.0 and Silverlight

## [3.2.0]

### Fixed
 - Fixed ContextPresevingGet for closed generic types [(#3)](https://github.com/ninject/Ninject.Extensions.ContextPreservation/issues/3)

## [3.0.1]

### Fixed
 - Fixed incompatibility with the factory extension, factories can now be resolved directly.

## [3.0.0]

### Added
 - Support for Ninject.Extensions.Factory

### Changed
 - Removed NoWeb builds because they are not needed anymore
 - Removed BindInterfaceToBinding use the new core functionality to bind multiple interfaces instead. (Bind<IA, IB>())
