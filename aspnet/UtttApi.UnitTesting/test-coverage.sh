# script to generate all the coverage stuff

readonly results_dir="CoverageResults"
readonly report_dir="CoverageReport"

cd "$( cd "$( dirname "${BASH_SOURCE[0]}" )" >/dev/null 2>&1 && pwd )" # path to the script
rm -rf $results_dir # remove previous test results

dotnet test --collect:"XPlat Code Coverage" -r:$results_dir

readonly cobertura=$( find "$results_dir" -type f -name 'coverage.cobertura.xml' )
reportgenerator -reports:$cobertura -targetdir:$report_dir -reporttypes:Html

open $report_dir/index.html