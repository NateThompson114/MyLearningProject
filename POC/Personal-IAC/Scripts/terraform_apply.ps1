cd "C:/source/MyLearningProject/POC/Personal-IAC/Terraform"
if (Test-Path -Path "tfplan") {
    terraform apply tfplan
} else {
    Write-Host "Plan file not found. Run terraform plan first."
}
