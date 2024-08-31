#!/bin/bash

# Define the source directories
BACKEND_DIR="${HOME}/Diuna/backend"
RPI_SCRIPTS_DIR="${HOME}/Diuna/rpi_scripts"

# Define the target directories
TARGET_DIR="${HOME}/Diuna/code"
TARGET_CS_DIR="${TARGET_DIR}/cs"
TARGET_PYTHON_DIR="${TARGET_DIR}/python"

# Create target directories if they don't exist
mkdir -p "${TARGET_CS_DIR}"
mkdir -p "${TARGET_PYTHON_DIR}"

# Find and copy all .cs files, excluding specific unwanted files
find "${BACKEND_DIR}" -name "*.cs" \
    ! -name "AssemblyInfo.cs" \
    ! -name "*Generated.cs" \
    ! -name "*Designer.cs" \
    ! -path "*/obj/*" \
    ! -path "*/bin/*" \
    -exec cp {} "${TARGET_CS_DIR}/" \;

# Copy all Python scripts from the rpi_scripts directory (ignore subfolders)
find "${RPI_SCRIPTS_DIR}" -maxdepth 1 -name "*.py" -exec cp {} "${TARGET_PYTHON_DIR}/" \;

echo "Files copied successfully, ignoring generated files and subfolders!"
