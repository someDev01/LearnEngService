import { Search } from "lucide-react";
import styles from '../search/search_content.module.css';
function SearchContent({query, setQuery, placeholder}){
    return(
        <div className={styles.block_input}>
            <Search 
                size={18} 
                color='#515151'
                style={{position:'absolute', left: '8px'}}
            />
            <input 
                placeholder={placeholder}
                value={query}
                onChange={(e) => setQuery(e.target.value)}
                maxLength={24}
            />
        </div>
    )
}

export default SearchContent;