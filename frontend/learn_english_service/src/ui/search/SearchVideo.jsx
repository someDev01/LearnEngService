import { Search } from "lucide-react";
import styles from '../search/search_video.module.css';

function SearchVideo({query, setQuery}){
    return(
        <div className={styles.block_input}>
            <Search 
                size={18} 
                color='#515151'
                style={{position:'absolute', left: '8px'}}
            />
            <input 
                placeholder="поиск контента"
                value={query}
                onChange={(e) => setQuery(e.target.value)}
                maxLength={24}
            />
        </div>
    )
}

export default SearchVideo;