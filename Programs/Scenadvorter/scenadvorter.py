import xml.etree.ElementTree as ET

# Sort publisher list by counter
def listSorter(obj1, obj2):
    return obj2[1].__cmp__(obj1[1])

# Load the XML and get root entry
# Load the ADVANCE SCENE DAT file
# Download the lastest from here:
# http://www.advanscene.com/offline/datas/ADVANsCEne_NDS.zip
tree = ET.parse('ADVANsCEne_NDS.xml')
root = tree.getroot()

# Iterate over game and count publisher
publisher = {}
games = tree.find('games')
for g in games.iter('game'):
    # Get publisher name
    pub = g.find('publisher').text

    # Get publisher game counter
    count = 0
    if pub in publisher:
        count = publisher[pub]

    # Update counter
    publisher[pub] = count + 1

# Get top 10
pubList = []
for p in publisher:
    pubList.append([p, publisher[p]])

pubList = sorted(pubList, cmp=listSorter)

# Print top 10
print "Top 10 publishers:"
for i in range(0, 10):
    print "%02d.- %s (%d)" % (i + 1, pubList[i][0], pubList[i][1])
