import React from 'react';
import { Card, Row } from 'reactstrap';
import ContextMenuContainer from 'containers/pages/ContextMenuContainer';
import Pagination from 'containers/pages/Pagination';
import CanisterLookupThumbListView from 'containers/pages/CanisterLookupThumbListView';
import { Colxx } from 'components/common/CustomBootstrap';

function collect(props) {
  return { data: props.data };
}

const CanisterLookupListing = ({
  items,
  allCanisterLookupItems,
  displayMode,
  selectedItems,
  onCheckItem,
  currentPage,
  totalPage,
  onContextMenuClick,
  onContextMenu,
  onChangePage,
}) => {
  return (
    <Row>
      <Colxx xxs="12">
        <Card>
          <div className="pl-2 d-flex flex-grow-1 min-width-zero">
            <div className="card-body align-self-center d-flex flex-column flex-lg-row justify-content-between min-width-zero align-items-lg-center">
              <p className="w-10 list-item-heading mb-1 truncate font-weight-bold">
                Code
              </p>
              <p className="w-15 list-item-heading mb-1 truncate font-weight-bold">
                SKU
              </p>
              <p className="w-20 list-item-heading mb-1 truncate font-weight-bold">
                Name
              </p>
              <p className="w-15 list-item-heading mb-1 truncate font-weight-bold text-center">
                Created Date
              </p>
              <p className="w-10 list-item-heading mb-1 truncate font-weight-bold">
                Is Active ?
              </p>
            </div>
          </div>
        </Card>
      </Colxx>
      {items.map((product) => {
        if (displayMode === 'thumblist') {
          return (
            <CanisterLookupThumbListView
              key={product.ID}
              product={product}
              isSelect={selectedItems.includes(product.ID)}
              collect={collect}
              onCheckItem={onCheckItem}
            />
          );
        }
        return <></>;
      })}
      {allCanisterLookupItems === null ||
        (items.length === 0 && (
          <h3 className="mb-3 mt-3 w-100 text-center">No Data Found</h3>
        ))}
      <Pagination
        currentPage={currentPage}
        totalPage={totalPage}
        onChangePage={(i) => onChangePage(i)}
      />
      <ContextMenuContainer
        onContextMenuClick={onContextMenuClick}
        onContextMenu={onContextMenu}
      />
    </Row>
  );
};

export default React.memo(CanisterLookupListing);
