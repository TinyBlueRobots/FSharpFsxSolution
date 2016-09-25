if [ "$OS" = "Windows_NT" ]; then mono=""; else mono="mono"; fi

function execute {
  $@
  exit_code=$?
  if [ $exit_code -ne 0 ]; then
	  exit $exit_code
  fi
}
args=$@
if [ "$1" = "-h" ] ; then
  args=${@:2}
  rm -rf paket-files
  execute $mono .paket/paket.bootstrapper.exe
  execute $mono .paket/paket.exe install
  execute $mono .paket/paket.exe generate-include-scripts framework net45 type fsx
fi
$mono ./packages/FAKE/tools/FAKE.exe ./build.fsx $args