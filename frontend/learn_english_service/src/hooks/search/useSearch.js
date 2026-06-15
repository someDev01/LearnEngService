import { useEffect, useState } from "react";

const timeoutSearch = 500;
export const useSearch = ({
    fetchFunction,
    extractData
}) => {
    const [query, setQuery] = useState("");
    const [searchResults, setSearchResults] = useState([]);
    const [isLoading, setIsLoading] = useState(false);
    const [hasSearched, setHasSearched] = useState(false);
    const [hasMore, setHasMore] = useState(false);
    const [totalSearched, setTotalSearched] = useState(0);

    const normalisedQuery = query.trim();

    useEffect(() => {
        if(normalisedQuery.length < 2){
            setSearchResults([]);
            setHasSearched(false);
            setIsLoading(false);
            return;
        }

        setIsLoading(true);

        const timeout = setTimeout(async() => {
            const response = await fetchFunction(normalisedQuery, 1);

            if(response.success){
                const data = extractData(response);
                setSearchResults(data);
                setTotalSearched(response.data.totalCount);

                setHasMore(response.data.page < response.data.totalPages);
            }
            else{
                setSearchResults([]);
            }

            setHasSearched(true);
            setIsLoading(false);
        }, timeoutSearch);

        return () => clearTimeout(timeout);

    }, [normalisedQuery]);

    return{
        query,
        setQuery,
        searchResults,
        setSearchResults,
        totalSearched,
        isLoading,
        setIsLoading,
        hasSearched,
        hasMore,
        setHasMore,
        normalisedQuery
    };
};