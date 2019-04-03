export class DisplayImage {
	directory: string;
	url: string;
	indexOfArrayDebugString: string;
	
	constructor(path: string, indexOfArrayDebugString: string) {
		this.url = `url('/${path}')`;
		this.indexOfArrayDebugString = indexOfArrayDebugString;

		let split = path.split('/');
		if (split.length === 3) {
			this.directory = split[1];
		}
	}
}
