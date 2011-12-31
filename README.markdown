<strong>Pinger!</strong> is a tiny ping utility (13KB) that is used to quickly ping a large list of hosts and save the resulting successful and failure host lists into a text file.
<h2>Why?</h2>
When developing scripts and applications that will touch a large number of hosts,  if many of those hosts are offline or no longer exist, the timeouts involved with this can slow down execution time and dramatically slow down your script or application.

In my case, I was presented with a list of 600+ servers that needed to have the exact same script run against them, a successful server took only several milliseconds to finish,  a failed host took around 4 seconds to time out, clearly I needed to find out which servers were alive and which weren't and further again I needed to list the successful servers in a clean text file, 1 host per line, so I could import this into my application as an array.

And hence Pinger! was born :)
<h2>Usage</h2>
Very simple, Pinger.exe when run looks for hosts.txt in the same directory. Simply enter in your list of host names in this text file,  one per line and Pinger! will run over them 1 by 1 and output the results.
Simply extract the .zip containing Pinger.exe and hosts.txt to the same directory and open Pinger.exe.

<strong>Options</strong>

<ol>
	<li><span style="font-weight: normal;"><strong> </strong>Select option 1 to just ping the hosts found in hosts.txt and output the success and failure text files.</span></li>
	<li><span style="font-weight: normal;">Select option 2 to also include the info_info.txt file which lists the IP addresses of the host names.  (Slightly slower for large lists)</span></li>
</ol>

<h2>Output</h2>
3 files are outputted.  these are:
<ul>
	<li><strong>success.txt</strong> - The list of successfully pinged hosts, in per line format</li>
	<li><strong>failure.txt</strong> - The list of hosts that failed to ping, in line by line format.</li>
	<li><strong>ip_info.txt</strong> - If option 2 is selected, this will list the hostname and the IP address's assigned to this host.</li>
</ul>
<p style="text-align: center;">&nbsp;</p>
