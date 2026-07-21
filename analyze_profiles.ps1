$content = Get-Content 'Assets\Scripts\Data\MasterDatingPool.asset'
$total = 0
$realProfiles = 0
$rels = @{0=0; 1=0; 2=0}
$smokers = @{0=0; 1=0}
$pets = @{0=0; 1=0}

foreach ($line in $content) {
    if ($line -match '^\s*-\s+bio:\s*(.+)$') { $realProfiles++ }
    if ($line -match '^\s*-\s+') { $total++ }
    if ($line -match 'relationshipType: (\d+)') { $rels[[int]$matches[1]]++ }
    if ($line -match 'smokerStatus: (\d+)') { $smokers[[int]$matches[1]]++ }
    if ($line -match 'petStatus: (\d+)') { $pets[[int]$matches[1]]++ }
}

Write-Host "Analysis of MasterDatingPool.asset"
Write-Host "----------------------------------"
Write-Host "Total Profiles in Pool: $total"
Write-Host "Profiles with Bio:      $realProfiles"
Write-Host "Empty/Default Profiles: $($total - $realProfiles)"

Write-Host "`nRelationship Type Distribution:"
Write-Host "  LongTerm:   $($rels[0])"
Write-Host "  Casual:     $($rels[1])"
Write-Host "  Friendship: $($rels[2])"

Write-Host "`nSmoker Status Distribution:"
Write-Host "  Smoker:     $($smokers[0])"
Write-Host "  NonSmoker:  $($smokers[1])"

Write-Host "`nPet Status Distribution:"
Write-Host "  HasPets:    $($pets[0])"
Write-Host "  NoPets:     $($pets[1])"
